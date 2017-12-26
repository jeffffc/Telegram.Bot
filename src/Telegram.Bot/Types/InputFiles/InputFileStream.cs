﻿using System.IO;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InputFiles
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class InputFileStream : IInputFile
    {
        /// <summary> 
        /// File content to upload
        /// </summary>
        public Stream Content { get; protected set; }

        /// <summary>
        /// Name of a file to upload using multipart/form-data
        /// </summary>
        public string FileName { get; set; }

        /// <inheritdoc cref="IInputFile.FileType"/>
        public virtual FileType FileType => FileType.Stream;

        /// <summary>
        /// ToDo
        /// </summary>
        protected InputFileStream()
        { }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="content"></param>
        public InputFileStream(Stream content)
            : this(content, default)
        { }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public InputFileStream(Stream content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="stream"></param>
        public static implicit operator InputFileStream(Stream stream)
            => new InputFileStream(stream);
    }
}
