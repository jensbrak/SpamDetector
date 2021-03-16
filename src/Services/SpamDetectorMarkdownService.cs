using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Piranha;

namespace Zon3.SpamDetector.Services
{
    public class SpamDetectorMarkdownService
    {
        private readonly string EXT=".md";

        private readonly string _path;

        private readonly IFileProvider _fileProvider;

        private Dictionary<string, string> _transformations;

        public SpamDetectorMarkdownService(IFileProvider fileProvider, string path)
        {
            _path = path;
            _fileProvider = fileProvider;
            _transformations =  GetTransformations();
        }

        private Dictionary<string, string> GetTransformations()
        {
            var files = _fileProvider.GetDirectoryContents(".").Where(f => f.Name.EndsWith(EXT));
            var names = files.Select(f => Path.GetFileNameWithoutExtension(f.Name));
            var transformations = names.ToDictionary(n => n, m => Transform(m));
            return transformations;
        }

        private string Transform(string markdown)
        {
            var filename = Path.Combine(_path, $"{markdown}{EXT}");
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
            return  Transform(markdown);
        }
    }
}
