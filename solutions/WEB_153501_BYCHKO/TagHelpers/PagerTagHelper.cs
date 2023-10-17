using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_153501_BYCHKO.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _contextAccessor;
        public string CurrentPage { get; set; }
        public string TotalPages { get; set; }
        public string? Category {  get; set; }

        public bool? Admin {  get; set; }=false;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor)
        {
            _linkGenerator = linkGenerator;
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string prevPage = CurrentPage == "1" ? "1" : (Convert.ToInt32(CurrentPage) -1).ToString();
            string nextPage = CurrentPage == TotalPages ? $"{TotalPages}" : (Convert.ToInt32(CurrentPage) + 1).ToString();
            output.TagName = "ul";   

            output.Attributes.SetAttribute("class", "pagination");
            if (Category != null) 
            {
                // кнопка взад начало
                var backButton = new TagBuilder("li");
                // чтобы тег был не самозакрывающимся
                backButton.TagRenderMode = TagRenderMode.Normal;

                // сss 
                backButton.AddCssClass("page-item");

                var backButtonAnchor = new TagBuilder("a");
                // чтобы тег был не самозакрывающимся
                backButtonAnchor.TagRenderMode = TagRenderMode.Normal;

                // сss 
                backButtonAnchor.AddCssClass("page-link");

                // атрибуты
                backButtonAnchor.MergeAttribute("href", _linkGenerator.GetPathByPage(_contextAccessor.HttpContext, values: new { pageno = prevPage, category = Category }));

                // текст внутри тега а
                backButtonAnchor.InnerHtml.AppendHtml("Previous");


                backButton.InnerHtml.AppendHtml(backButtonAnchor);
                
                output.Content.AppendHtml(backButton);
                //кнопка взад конец


                // страницы начало
                for (int i = 1; i <= Convert.ToInt32(TotalPages); i++)
                {
                    var certainPage = new TagBuilder("li");
                    // чтобы тег был не самозакрывающимся
                    certainPage.TagRenderMode = TagRenderMode.Normal;


                    certainPage.AddCssClass("page-item");
                    //добавляем выделение нынешнему тегу
                    if (Convert.ToInt32(CurrentPage) == i)
                        certainPage.AddCssClass("active");


                    var certainPageAnchor = new TagBuilder("a");
                    // чтобы тег был не самозакрывающимся
                    certainPageAnchor.TagRenderMode = TagRenderMode.Normal;

                    // сss 
                    certainPageAnchor.AddCssClass("page-link");

                    // атрибуты
                    certainPageAnchor.MergeAttribute("href", _linkGenerator.GetPathByPage(_contextAccessor.HttpContext, values: new { pageno = i, category = Category }));

                    // текст внутри тега а
                    certainPageAnchor.InnerHtml.AppendHtml(i.ToString());

                    //добавляем тег а в li
                    certainPage.InnerHtml.AppendHtml(certainPageAnchor);

                    //добавляем li в ul
                    output.Content.AppendHtml(certainPage);
                }
                // страницы конец

                //кнопка вперед начало
                var frontButton = new TagBuilder("li");
                // чтобы тег был не самозакрывающимся
                frontButton.TagRenderMode = TagRenderMode.Normal;

                // сss 
                frontButton.AddCssClass("page-item");

                var frontButtonAnchor = new TagBuilder("a");
                // чтобы тег был не самозакрывающимся
                frontButtonAnchor.TagRenderMode = TagRenderMode.Normal;

                // сss 
                frontButtonAnchor.AddCssClass("page-link");

                // атрибуты
                frontButtonAnchor.MergeAttribute("href", _linkGenerator.GetPathByPage(_contextAccessor.HttpContext, values: new { pageno = nextPage, category = Category }));
                // текст внутри тега а
                frontButtonAnchor.InnerHtml.AppendHtml("Next");


                frontButton.InnerHtml.AppendHtml(frontButtonAnchor);

                output.Content.AppendHtml(frontButton);
                //кнопка вперед конец
            }
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
