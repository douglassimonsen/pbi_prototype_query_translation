import sys
import clr  # type: ignore[import-untyped]

sys.path.append(r"C:\Users\USER\source\repos\ConsoleApp3\ConsoleApp3\bin\x64\Debug")
clr.AddReference("PrototypeQueryTranslation")  # type: ignore
from Translation import PrototypeQueryTranslation

query = open(r"C:\Users\USER\Documents\repos\pbyx\data.json").read()

# x = PrototypeQueryTranslation.GetPath(r"C:\Users\USER\source\repos\ConsoleApp3\ConsoleApp3\bin\x64\Debug")
x = PrototypeQueryTranslation.Translate(
    query,
    "d31a4306-acbb-469b-aa5f-52c0ab162af0",
    51184,
    r"C:\Users\USER\source\repos\ConsoleApp3\ConsoleApp3\bin\x64\Debug"
)
print(x.DaxExpression)