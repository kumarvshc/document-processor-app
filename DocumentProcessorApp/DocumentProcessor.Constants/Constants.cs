namespace DocumentProcessor.Constants
{
    public static class Constants
    {
        public const string CONST_KEYSCAN_QUEUE_NAME = "scan-documents";
        public const string CONST_PATTERN_EXTRACT_QUEUE_NAME = "pattern-documents";
        public const string CONST_KEYWORD_SCAN = "dangerous";
        public const string CONST_REGEX_PATTERN = @"[A-Z]{2,3}\d{3,5}";
        public const string CONST_DOC_PROCESSOR_API_BASEURL = @"https://localhost:7088";
        public const string CONST_DOC_PROCESSOR_API_ADD_DOCUMENT_POST_URL = @"/api/document";
    }
}
