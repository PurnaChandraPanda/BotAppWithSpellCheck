## v3 C# bot app that talks to bing spell check service
1. Add nuget package "Microsoft.Azure.CognitiveServices.Language.SpellCheck"
2. Bing Spell check API check in C#

```
        private async Task<string> SpellCheckAsync(string inputText)
        {
            var client = new SpellCheckClient(new ApiKeyServiceClientCredentials(ConfigurationManager.AppSettings["BingSpellCheckKey"]));

            //var response = await client.SpellCheckerWithHttpMessagesAsync(inputText);
            /*
             * For SpellCheck mode, The default is Proof. 1) Proof—Finds most spelling and grammar mistakes. 
             * 2) Spell—Finds most spelling mistakes but does not find some of the grammar errors that Proof
             * catches (for example, capitalization and repeated words). Possible values
             * include: 'proof', 'spell'
             * Ref SDK - https://github.com/Azure/azure-sdk-for-net/blob/psSdkJson6/src/SDKs/CognitiveServices/dataPlane/Language/SpellCheck/BingSpellCheck/Generated/SpellCheckClient.cs
             * */
            var response = await client.SpellCheckerWithHttpMessagesAsync(text: inputText,
                                                            mode: "spell", acceptLanguage: "en-US");

            //Get the service task result
            var result = response.Response;

            var spellcheckContent = await result.Content.ReadAsStringAsync();

            return spellcheckContent;
        }
```

Note: The whole of logic is dealt via "SpellCheckClient" class. 