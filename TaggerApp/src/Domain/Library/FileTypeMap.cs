namespace Domain.Library {
    public static class FileTypeMap {
        public record FileTypeInfo(string extension, string type, string fullName) {
            public readonly string Extension = extension;
            public readonly string Type = type;
            public readonly string FullName = fullName;
        }

        private static List<FileTypeInfo> fileTypes = [
            // image
            new("jpg", "image", "Joint Photographic Experts Group"),
            new("jpeg", "image", "Joint Photographic Experts Group"),
            new("png", "image", "Portable Network Graphics"),
            new("webp", "image", "Web Picture"),
            new("bmp", "image", "Bitmap"),
            new("tiff", "image", "Tag Image File Format"),

            // video
            new("gif", "video", "Graphics Interchange Format"),
            new("mp4", "video", "Moving Picture Experts Group Video Layer 4"),
            new("avi", "video", "Audio Video Interleave"),
            new("mkv", "video", "Matroska Video"),
            new("mov", "video", "Quicktime Movie File"),
            new("wmv", "video", "Windows Media Video"),

            // audio
            new("mp3", "audio", "Moving Picture Experts Group Audio Layer 3"),
            new("wav", "audio", "Waveform Audio File"),
            new("flac", "audio", "Free Lossless Audio Codec"),
            new("aac", "audio", "Advanced Audio Coding"),
            new("ogg", "audio", "Ogg Vorbis Compressed Audio File"),

            // text
            new("txt", "text", "Plain Text"),
            new("csv", "text", "Comma-separated Values"),
            new("md", "text", "Markdown"),
            new("log", "text", "Log"),

            // code
            new("js", "code", "Javascript"),
            new("cs", "code", "C# Source"),
            new("java", "code", "Java Source"),
            new("py", "code", "Python Source"),
            new("html", "code", "Hypertext Markup Language"),
            new("css", "code", "Cascading Style Sheets"),

            // executable
            new("exe", "executable", "Windows Executable"),
            new("dll", "executable", "Dynamic Link Library (DLL)"),

            // document
            new("pdf", "document", "Portable Document Format"),
            new("docx", "document", "Word Document"),
            new("xlsx", "document", "Excel Spreadsheet"),
            new("pptx", "document", "Powerpoint Presentation"),
        ];

        public static FileTypeInfo GetFileTypeInfo(string path) {
            var extension = Path.GetExtension(path).Substring(1);
            var result = fileTypes.Find(f => f.Extension == extension);

            return result ?? new FileTypeInfo(extension, "unknown", "unknown");
        }

        public static string GetFileType(string path) {
            return GetFileTypeInfo(path).Type;
        }
    }
}