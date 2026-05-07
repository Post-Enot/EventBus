using System;
using System.Text;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    public class CodeGenerator
    {
        public CodeGenerator() => _stringBuilder.AppendLine(
$@"// ------------------------------------------------------------------------------
// 
// Этот файл был полностью сгенерирован.
// Любые ручные изменения будут перезаписаны при следующей генерации.
// 
// Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm:ss}
// Генератор: IUP.Toolkits.Editor.CodeGenerator
// ------------------------------------------------------------------------------
");

        public readonly struct BracketsBlock : IDisposable
        {
            public BracketsBlock(CodeGenerator codeGenerator)
            {
                _codeGenerator = codeGenerator;
                _codeGenerator.OpenBracket();
            }

            private readonly CodeGenerator _codeGenerator;

            public void Dispose() => _codeGenerator.CloseBracket();
        }

        public const string StrIndent = "    ";

        public int Indent { get; private set; }

        private readonly StringBuilder _stringBuilder = new();

        public CodeGenerator Using(ReadOnlySpan<char> directoryName)
        {
            AppendIndent();
            _stringBuilder.Append("using ");
            _stringBuilder.Append(directoryName);
            _stringBuilder.Append(";");
            _stringBuilder.AppendLine();
            return this;
        }

        public CodeGenerator Using(ReadOnlySpan<char> pseudonym, ReadOnlySpan<char> element)
        {
            AppendIndent();
            _stringBuilder.Append("using ");
            _stringBuilder.Append(pseudonym);
            _stringBuilder.Append(" = ");
            _stringBuilder.Append(element);
            _stringBuilder.Append(';');
            _stringBuilder.AppendLine();
            return this;
        }

        public CodeGenerator UsingStatic(ReadOnlySpan<char> element)
        {
            AppendIndent();
            _stringBuilder.Append("using static ");
            _stringBuilder.Append(element);
            _stringBuilder.Append(';');
            _stringBuilder.AppendLine();
            return this;
        }

        public CodeGenerator Empty()
        {
            _stringBuilder.AppendLine();
            return this;
        }

        public BracketsBlock BlockBracket() => new(this);

        public BracketsBlock BlockBracket(ReadOnlySpan<char> line)
        {
            AppendIndent();
            _stringBuilder.Append(line);
            _stringBuilder.AppendLine();
            return BlockBracket();
        }

        public BracketsBlock BlockNamespace(ReadOnlySpan<char> namespaceName)
        {
            AppendIndent();
            _stringBuilder.Append("namespace ");
            _stringBuilder.Append(namespaceName);
            _stringBuilder.AppendLine();
            return BlockBracket();
        }

        public CodeGenerator AddLines(params string[] lines)
        {
            foreach (string line in lines)
            {
                AppendIndent();
                _ = _stringBuilder.AppendLine(line);
            }
            return this;
        }

        public CodeGenerator AddLine(string line)
        {
            AppendIndent();
            _stringBuilder.AppendLine(line);
            return this;
        }

        public void OpenBracket()
        {
            AppendIndent();
            _stringBuilder.AppendLine("{");
            Indent += 1;
        }

        public void CloseBracket()
        {
            Indent -= 1;
            AppendIndent();
            _stringBuilder.AppendLine("}");
        }

        public override string ToString() => _stringBuilder.ToString();

        public void AppendIndent()
        {
            for (int i = 0; i < Indent; i += 1)
            {
                _stringBuilder.Append(StrIndent);
            }
        }
    }
}
