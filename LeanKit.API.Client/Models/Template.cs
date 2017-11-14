using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeanKit.Models
{
	public class TemplateListResponse
	{
		public TemplateListResponse()
		{
			Categories = new List<Category>();
		}
		public List<Category> Categories { get; set; }

		public class Category
		{
			public Category()
			{
				Templates = new List<Template>();
			}
			public long Id { get; set; }
			public string Name { get; set; }
			public List<Template> Templates { get; set; }
		}

		public class Template
		{
			public long Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public bool IsEnabled { get; set; }
			public bool IsGlobal { get; set; }
		}
	}

	public class TemplateCreateRequest
	{
        [JsonConverter(typeof(Utils.BigIntConverter))]
		public long BoardId { get; set; }
		public string TemplateName { get; set; }
		public string TemplateDescription { get; set; }
		public bool IncludeCards { get; set; }
	}

    public class TemplateCreateResponse
    {
        public long Id { get; set; }
    }
}
