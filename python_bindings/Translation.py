import sys
import clr  # type: ignore[import-untyped]

source_folder =r"C:\Users\USER\source\repos\ConsoleApp3\ConsoleApp3\bin\x64\Debug"
sys.path.append(source_folder)
clr.AddReference("PrototypeQueryTranslation")  # type: ignore
from Translation import PrototypeQueryTranslation

query = open(r"C:\Users\USER\Documents\repos\pbyx\data.json").read()

# x = PrototypeQueryTranslation.GetPath(r"C:\Users\USER\source\repos\ConsoleApp3\ConsoleApp3\bin\x64\Debug")
x = PrototypeQueryTranslation.Translate(
    query,
    "d31a4306-acbb-469b-aa5f-52c0ab162af0",
    51184,
    source_folder
)
print(x.DaxExpression)