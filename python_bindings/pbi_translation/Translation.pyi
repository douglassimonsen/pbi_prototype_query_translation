class DataViewQueryTranslationResult:
    DaxExpression: str
    SelectNameToDaxColumnName: dict[str, str]

class PrototypeQuery:
    @staticmethod
    def Translate(query: str, dbName: str, port: int, workingDirectory: str | None = None) -> DataViewQueryTranslationResult: 
        ...