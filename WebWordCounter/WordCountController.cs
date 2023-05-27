using Microsoft.AspNetCore.Mvc;

namespace WebWordCounter
{
    public class WordCountController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody]string text)
        {
            WordCounter.WordCounter wordCounter = new WordCounter.WordCounter();
            var wordCountsDesc = wordCounter.CountWordsMultiThreaded(text);

            return Ok(wordCountsDesc);
        }

    }
}
