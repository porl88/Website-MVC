// https://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

namespace MVC.Core.Data.EntityFramework
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Entities.Account;
    using Entities.Article;
    using Entities.Culture;
    using Entities.Location;
    using Entities.Website;
    using Entities.Website.PageItem;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly WebsiteDbContext context = new WebsiteDbContext();
        private readonly IRepository<Article, int> articleRepository;
        private readonly IRepository<ArticleVersion, int> articleVersionRepository;
        private readonly IReadOnlyRepository<Country, string> countryRepository;
        private readonly IReadOnlyRepository<Language, string> languageRepository;
        private readonly IRepository<Page, int> pageRepository;
        private readonly IRepository<PageVersion, int> pageVersionRepository;
        private readonly IRepository<PlainText, int> plainTextRepository;
        private readonly IRepository<RichText, int> richTextRepository;
        private readonly IRepository<User, int> userRepository;

        public UnitOfWork()
        {
#if DEBUG
            // http://www.codeproject.com/Tips/814618/Use-of-Database-SetInitializer-method-in-Code-Firs
            //this.context.Database.Initialize(false);
            // https://blog.oneunicorn.com/2013/05/08/ef6-sql-logging-part-1-simple-logging/
            this.context.Database.Log = s => Debug.WriteLine(s);
#endif
            this.articleRepository = new EntityFrameworkRepository<Article, int>(this.context);
            this.articleVersionRepository = new EntityFrameworkRepository<ArticleVersion, int>(this.context);
            this.countryRepository = new EntityFrameworkReadOnlyRepository<Country, string>(this.context);
            this.languageRepository = new EntityFrameworkReadOnlyRepository<Language, string>(this.context);
            this.pageRepository = new EntityFrameworkRepository<Page, int>(this.context);
            this.pageVersionRepository = new EntityFrameworkRepository<PageVersion, int>(this.context);
            this.plainTextRepository = new EntityFrameworkRepository<PlainText, int>(this.context);
            this.richTextRepository = new EntityFrameworkRepository<RichText, int>(this.context);
            this.userRepository = new EntityFrameworkRepository<User, int>(this.context);
        }

        public IRepository<Article, int> ArticleRepository => this.articleRepository;

        public IRepository<ArticleVersion, int> ArticleVersionRepository => this.articleVersionRepository;

        public IReadOnlyRepository<Country, string> CountryRepository => this.countryRepository;

        public IReadOnlyRepository<Language, string> LanguageRepository => this.languageRepository;

        public IRepository<Page, int> PageRepository => this.pageRepository;

        public IRepository<PageVersion, int> PageVersionRepository => this.pageVersionRepository;

        public IRepository<PlainText, int> PlainTextRepository => this.plainTextRepository;

        public IRepository<RichText, int> RichTextRepository => this.richTextRepository;

        public IRepository<User, int> UserRepository => this.userRepository;

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
