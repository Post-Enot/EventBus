# Event Bus

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity](https://img.shields.io/badge/Unity-6000.0.0%2B-blue)](https://unity.com/)

**Типизированная шина событий** для Unity с поддержкой вложенных вызовов, безопасной модификацией во время отправки, гибким управлением подписками и встроенным логированием ошибок.

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

private IEventBus EventBus => _eventBusReference.EventBus;
```

### 4. Подпишись на события через `IEventReceiver`

```csharp
private IEventReceiver _receiver;

private void Awake() => _receiver = EventBus.CreateReceiver().Register<PlayerDiedEvent>(OnPlayerDied);

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

## Общие характеристики

Шина событий полностью реализована на основе делегатов и типизации событий через контекстный тип.
Контекстным типом может быть как структура, так и класс, в зависимости от потребностей; на данный момент использование `ref struct` в качестве контекстного типа невозможно.
Контекстный тип не имеет каких-либо иных ограничений и может содержать как поля и свойства, так и методы; тем не менее, большую часть времени вы скорее всего будете
использовать пустые типы.
