﻿using Microsoft.Extensions.FileProviders;
using Piranha;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zon3.SpamDetector.Services
{
    public class SpamDetectorMarkdownService
    {
        private const string Extension = ".md";

        private readonly string _path;

        private readonly IFileProvider _fileProvider;

        private readonly Dictionary<string, string> _transformations;

        public SpamDetectorMarkdownService(IFileProvider fileProvider, string path)
        {
            _path = path;
            _fileProvider = fileProvider;
            _transformations =  GetTransformations();
        }

        private Dictionary<string, string> GetTransformations()
        {
            var files = _fileProvider.GetDirectoryContents(".").Where(f => f.Name.EndsWith(Extension));
            var names = files.Select(f => Path.GetFileNameWithoutExtension(f.Name));
            var transformations = names.ToDictionary(n => n, Transform);
            return transformations;
        }

        private string Transform(string markdown)
        {
            var filename = Path.Combine(_path, $"{markdown}{Extension}");
            var fileInfo = _fileProvider.GetFileInfo(filename);
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            var html = App.Markdown.Transform(reader.ReadToEnd());
            return html;
        }

        public string ToHtml(string markdown)
        {
            // Get stored transformation
            if (_transformations.TryGetValue(markdown, out string html))
            {
                return html;
            }

            // Store and return
            return Transform(markdown);
        }
    }
}
