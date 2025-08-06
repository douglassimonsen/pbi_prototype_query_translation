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
    query = open(r"C:\Users\USER\Documents\repos\pbyx\data.json").read()
    x = prototype_query(
        query,
        "78eab298-6cae-4b27-a8ed-a54ece56023c",
        55706
    )
    print(x.DaxExpression)