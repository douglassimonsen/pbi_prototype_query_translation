class DataViewQueryTranslationResult:
    DaxExpression: str
    SelectNameToDaxColumnName: dict[str, str]

class PrototypeQueryTranslation:
    @staticmethod
    def Translate(query: str, dbName: str, port: int, workingDirectory: str | None = None) -> DataViewQueryTranslationResult: ...