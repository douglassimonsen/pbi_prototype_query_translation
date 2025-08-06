import sys
import clr  # type: ignore[import-untyped]
from pathlib import Path
SOURCE_FOLDER = (Path(__file__).parent / 'libs').absolute().as_posix()
sys.path.insert(0, SOURCE_FOLDER)
clr.AddReference("Translation")  # type: ignore
from Translation import PrototypeQuery, DataViewQueryTranslationResult



def prototype_query(query: str, db_name: str, port: int) -> "DataViewQueryTranslationResult":
    return PrototypeQuery.Translate(
        query,
        db_name,
        port,
        SOURCE_FOLDER
    )


if __name__ == '__main__':
    query = open(r"C:\Users\USER\Documents\repos\prototype_parsing\data.json").read()
    x = prototype_query(
        query,
        "e1ff5407-9b29-4692-870b-41bacbb9c4f5",
        50025
    )
    print(x.DaxExpression)