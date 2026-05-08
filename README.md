# Event Bus

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity](https://img.shields.io/badge/Unity-6000.0.0%2B-blue)](https://unity.com/)

**Типизированная шина событий** для Unity с поддержкой вложенных вызовов, безопасной модификацией во время отправки, гибким управлением подписками и встроенным логированием ошибок.

**Особенности:**

1. Строго типизированная шина событий.
2. Удобный API для контроля жизненного цикла регистраций обратных вызовов.
3. Корректная работа с отключённым `Domain Reload`.

## 📦 Установка

### Через Unity Package Manager (рекомендуется)
1. Скопировать URL этого репозитория.
2. В Unity: **Window → Package Manager → + → Add package from git URL…**
3. Вставить ссылку и подтвердить.

## 🚀 Быстрый старт

### 1. Создай шину событий

Один из простейших способов работы с шиной событий заключается в создании `EventBusReference`. Для этого:


**Assets → Create → Post Enot → Event Bus Reference**.


Данный `ScriptableObject` инкапсулирует логику создания и настройки шины событий; его также удобно назначать через интерфейс и `SerializedField`-поля.

### 2. Создай событие (контекст)
```csharp
public readonly struct PlayerDiedEvent
{
    public readonly string PlayerName { get; }
    public readonly int Score { get; }
    
    public PlayerDiedEvent(string name, int score)
    {
        PlayerName = name;
        Score = score;
    }
}
```

### 3. Получи ссылку на шину

```csharp
[SerializeField] private EventBusReference _eventBusReference;
```

### 4. Подпишись на события через `IEventReceiver`

```csharp
private IEventReceiver _receiver;

private void Awake() => _receiver = _eventBusReference.EventBus.CreateReceiver().Register<PlayerDiedEvent>(OnPlayerDied);

private void OnEnable() => _receiver.Enable();

private void OnDisable() => _receiver.Disable();

private void OnDestroy() => _receiver.UnregisterAll();

private void OnPlayerDied(PlayerDiedEvent context)
    => Debug.Log($"Игрок: {context.PlayerName} умер со счётом: {context.Score}.");
```

### 5. Вызови событие

```csharp
PlayerDiedEvent context = new("Hero", 42);
_eventBus.Invoke(context);
```

### `EventBus`, `IEventBus`

`EventBus` - основной тип, инкапсулирующий логику изменения и хранения обратных вызовов. Вы можете создать шину событий вручную, используя конструктор и передав стандартный логгер:

```csharp
ILogger logger = UnityEngine.Debug.unityLogger;
IEventBus eventBus = new EventBus(logger);
```

Вы можете и вовсе не передавать логгер: это не повлияет на работоспособность шины и не приведёт к выбросу исключений, однако крайне не рекомендуется, так как в этом случае вы не увидите
логи о выброшенных исключениях во время обратных вызовов событий.

### `ILogger`, `UnityLoggerWrapper`, выброс исключений

Данные интерфейс создан исключительно с целью предоставления подмены используемого шиной событий логгера. Под капотом `UnityLoggerWrapper` просто обворачивает вызовы `UnityEngine.ILogger`.

На данный момент единственное место, где используется логгирование - обратные вызовы событий при `Invoke`. Для того, чтобы исключение, возникшее при обработке одного обратного вызова не предотвращало обрбаотку оставшихся обратных вызовов, при выбросе исключения оно ловится и логгируется с помощью `ILogger.LogException(exception)`.

### `EventBusReference`

Предоставляет простой встроенный механизм для протягивания зависимостей между компонентами. `EventBusReference` реализует ленивую инициализацию `EventBus` с использованием `UnityLoggerWrapper` в качестве логгера по умолчанию. Корректно работает с отключённым `Domain Reload`, сбрасывая состояние.

### События, типы событий

Шина событий полностью реализована на основе типизации событий через контекстный тип.

Контекстным типом может быть как структура, так и класс, в зависимости от потребностей; на данный момент использование `ref struct` в качестве контекстного типа невозможно.
Контекстный тип не имеет каких-либо иных ограничений и может содержать как поля и свойства, так и методы; тем не менее, большую часть времени вы скорее всего будете
использовать пустые типы.

**Пример:**

```csharp
// Пустой тип события.
public readonly struct Event { }

// Тип события, содержащий данные.
public readonly struct EventWithContext
{
    public EventWithContext(string? message) => Message = message;

    public readonly string? Message { get; }
}
```

### Обратные вызовы событий, `EventCallback`

В рамках данного пакета обратный вызов события - делегат, обрабатываемый при вызове события. Пакет предоставляет два делегата: `EventCallback` и `EventCallback<TEvent>`,
для обратных вызовов принимающих и не принимающих контекст соответственно.

**Пример:**
```csharp
eventBus.Register<GameEvent>(CallbackHandlerWithoutArgs);
eventBus.Register<GameEvent>(CallbackHandlerWithArgs);

private void CallbackHandlerWithoutArgs() { /* ... */ }

private void CallbackHandlerWithArgs(GameEvent context) { /* ... */ }
```

### Вызов событий

Вызов события возможен как с передачей, так и без передачи контекста.

```csharp
// Вызов с передачей контекста.
GameEvent context = new(arg0, arg1);
eventBus.Invoke(context);

// Вызов без передачи контекста.
eventBus.Invoke<GameEvent>();
```

Под капотом при вызове события без передачи контекста он всё же создаётся, с вызовом стандартного конструктора. Происходит примерно следующее:

```csharp
public void Invoke<TEvent>() where TEvent : new()
{
    TEvent context = new();
    Invoke(context);
}
```

В связи с этим вызов события без передачи контекста возможен только в том случае, если тип события имеет открытый конструктор без параметров.

### Жизненный цикл обратных вызовов

Контроль над жизненным циклом обратных вызовов полностью ложится на плечи пользователя: шина событий не использует внутри механизмы, схожие с `WeakReference` из соображений производительности.
Настоятельно рекомендуется контролировать и проверять моменты создания и отмены регистрации обратных вызовов событий.

### Манипуляторы событий: `IEventReceiver`

Порой вам необходимо обработать множество событий в одном компоненте. При использовании исключительно шины событий это может выглядеть следующим образом:

```csharp
[SerializeField] private IEventBusReference _eventBusReference;

private IEventBus EventBus => _eventBusReference.EventBus;

private void OnEnable()
{
    EventBus.Register<Event0>(OnEvent0);
    EventBus.Register<Event1>(OnEvent1);
    EventBus.Register<Event2>(OnEvent2);
    // ...
    EventBus.Register<EventN>(OnEventN);
}

private void OnDisable() => UnsubscribeFromEvents();

private void OnDestroy()
{
    if (!enabled)
    {
        return;
    }
    UnsubscribeFromEvents();
}

private void UnsubscribeFromEvents()
{
    EventBus.Unregister<Event0>(OnEvent0);
    EventBus.Unregister<Event1>(OnEvent1);
    EventBus.Unregister<Event2>(OnEvent2);
    // ...
    EventBus.Unregister<EventN>(OnEventN);
}

```

Код получается излишне шаблонным и громоздким. С целью упрощения контроля жизненного цикла обработчиков событий были создан манипулятор `IEventReceiver`. Он реализует логику хранения и обработки обратных вызовов событий. При использовании `IEventReceiver` код, представленный выше, превращается в следующий:

```csharp
[SerializeField] private IEventBusReference _eventBusReference;

private IEventReceiver _receiver;

private void Awake() _receiver = _eventBusReference.EventBus
        .CreateReceiver()
        .Register<Event0>(OnEvent0)
        .Register<Event1>(OnEvent1)
        .Register<Event2>(OnEvent2)
        // ...
        .Register<EventN>(OnEventN);

private void OnEnable() => _receiver.Enable();

private void OnDisable() => _receiver.Disable();

private void OnDestroy() => _receiver.UnregisterAll();
```

### Манипуляторы событий: `IEventInvoker`

`IEventInvoker` представляет собой класс-обёртку, предоставляя методы для вызова событий. Наиболее полезен для написания чистого кода и передачи классам/методам минимально необходимой логики, когда им необходимо вызывать события, но не управлять жизненным циклом обратных вызовов.

### Манипуляторы событий: `IEventBroker`

Представляет собой объединение `IEventReceiver` и `IEventInvoker`.

## Важные особенности реализации

### Порядок обратных вызовов событий

Обратные вызовы одного и того же события происходят в порядке их регистрации, вне зависимости оттого, принимает обработчик контекст или нет.

**Пример:**

```csharp
_broker
    .Register<Event>(OnEvent0)
    .Register<Event>(OnEvent1)
    .Register<Event>(OnEvent2);

_broker.Invoke<Event>();

private void OnEvent0() => Debug.Log("Event Handling 0 Complete.");

private void OnEvent1(Event context) => Debug.Log("Event Handling 1 Complete.");

private void OnEvent2() => Debug.Log("Event Handling 2 Complete.");

/* Результат вызова:
 * Event Handling 0 Complete.
 * Event Handling 1 Complete.
 * Event Handling 2 Complete.
 */

```

### Множественные регистрации обратных вызовов

Шина событий предоставляет возможность множественной регистрации одного и того же обратного вызова на одно и то же событие; тем не менее, это не является предпочтительной практикой.
При множественных регистраций одного и того же обратного вызова на одно и то же событие важно помнить, что методы `Unregister` всегда отменяют наиболее раннюю регистрацию.

### Создание и отмена регистраций при активном вызове

Если обработчик события вызывает методы создания/отмены регистрации на то же событие, обработка которого происходит в данный момент, то:

1. Отмена регистрации произойдёт моментально.
2. Создание регистрации будет отложено до конца обработки события.

**Пример моментальной отмены регистрации:**

```csharp
_broker.Register<Event>(OnEvent0);
_broker.Register<Event>(OnEvent1);

_broker.Invoke<Event>();

// ...

private void OnEvent0()
{
    _broker.Unregister<Event>(OnEvent1);
    Debug.Log("Event Handling 0 Complete.");
}

private void OnEvent1() => Debug.Log("Event Handling 1 Complete.");

/* Результат:
 * Event Handling 0 Complete.
 */
```

**Пример отложенного создания регистрации:**

```csharp
_broker.Register<Event>(OnEvent0);

_broker.Invoke<Event>();

// ...

private void OnEvent0()
{
    _broker.Register<Event>(OnEvent1);
    Debug.Log("Event Handling 0 Complete.");
}

private void OnEvent1() => Debug.Log("Event Handling 1 Complete.");

/* Результат:
 * Event Handling 0 Complete.
 */
```
