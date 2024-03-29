﻿﻿#region License
/*
Copyright 2015 Aylien, Inc. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aylien.TextApi
{
    public class Combined : Base
    {
        public Combined(Configuration config) : base(config) { }

        internal Response call(string url, string text, string[] endpoints)
        {
            List<Dictionary<string, string>> parameters = new List<Dictionary<string, string>>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters.Add(new Dictionary<string, string> { { "url", url } });

            if (!String.IsNullOrWhiteSpace(text))
                parameters.Add(new Dictionary<string, string> { { "text", text } });

            foreach (string endpoint in endpoints)
            {
                parameters.Add(new Dictionary<string, string> { { "endpoint", endpoint } });
            }

            Connection connection = new Connection(Configuration.Endpoints["Combined"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public CombinedResult[] Results { get; set; }
        public string Text { get; set; }

        public Classify Classify { get; set; }
        public Concepts Concepts { get; set; }
        public Entities Entities { get; set; }
        public Extract Extract { get; set; }
        public Hashtags Hashtags { get; set; }
        public Language Language { get; set; }
        public Sentiment Sentiment { get; set; }
        public Summarize Summarize { get; set; }

        private void populateData(string jsonString)
        {
            Combined m = JsonConvert.DeserializeObject<Combined>(jsonString);

            Text = m.Text;
            Results = m.Results;

            foreach (var item in m.Results)
            {
                switch (item.Endpoint)
                {
                    case "classify":
                        Classify = JsonConvert.DeserializeObject<Classify>(item.Result.ToString());
                        Classify.Text = Text;
                        break;
                    case "concepts":
                        Concepts = JsonConvert.DeserializeObject<Concepts>(item.Result.ToString());
                        Concepts.Text = Text;
                        break;
                    case "entities":
                        Entities = JsonConvert.DeserializeObject<Entities>(item.Result.ToString());
                        Entities.Text = Text;
                        break;
                    case "extract":
                        Extract = JsonConvert.DeserializeObject<Extract>(item.Result.ToString());
                        break;
                    case "hashtags":
                        Hashtags = JsonConvert.DeserializeObject<Hashtags>(item.Result.ToString());
                        Hashtags.Text = Text;
                        break;
                    case "language":
                        Language = JsonConvert.DeserializeObject<Language>(item.Result.ToString());
                        Language.Text = Text;
                        break;
                    case "sentiment":
                        Sentiment = JsonConvert.DeserializeObject<Sentiment>(item.Result.ToString());
                        Sentiment.Text = Text;
                        break;
                    case "summarize":
                        Summarize = JsonConvert.DeserializeObject<Summarize>(item.Result.ToString());
                        Summarize.Text = Text;
                        break;
                }
            }
        }
    }

    public class CombinedResult
    {
        public string Endpoint { get; set; }
        public dynamic Result { get; set; }
    }
}
