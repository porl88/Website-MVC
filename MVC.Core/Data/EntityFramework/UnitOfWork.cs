// https://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

namespace MVC.Core.Data.EntityFramework
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Entities.Account;
    using Entities.Article;
    using Entities.Culture;
    using Entities.Website;
    using Entities.Website.PageItem;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly WebsiteDbContext context = new WebsiteDbContext();
        private readonly IRepository<Article> articleRepository;
        private readonly IRepository<ArticleVersion> articleVersionRepository;
        private readonly IReadOnlyRepository<Language> languageRepository;
        private readonly IRepository<Page> pageRepository;
        private readonly IRepository<PageVersion> pageVersionRepository;
        private readonly IRepository<PlainText> plainTextRepository;
        private readonly IRepository<RichText> richTextRepository;
        private readonly IRepository<User> userRepository;

        public UnitOfWork()
        {
#if DEBUG
            // http://www.codeproject.com/Tips/814618/Use-of-Database-SetInitializer-method-in-Code-Firs
            //this.context.Database.Initialize(false);
            // https://blog.oneunicorn.com/2013/05/08/ef6-sql-logging-part-1-simple-logging/
            this.context.Database.Log = s => Debug.WriteLine(s);
#endif
            this.articleRepository = new EntityFrameworkRepository<Article>(this.context);
            this.articleVersionRepository = new EntityFrameworkRepository<ArticleVersion>(this.context);
            this.languageRepository = new EntityFrameworkReadOnlyRepository<Language>(this.context);
            this.pageRepository = new EntityFrameworkRepository<Page>(this.context);
            this.pageVersionRepository = new EntityFrameworkRepository<PageVersion>(this.context);
            this.plainTextRepository = new EntityFrameworkRepository<PlainText>(this.context);
            this.richTextRepository = new EntityFrameworkRepository<RichText>(this.context);
            this.userRepository = new EntityFrameworkRepository<User>(this.context);
        }

        public IRepository<Article> ArticleRepository => this.articleRepository;

        public IRepository<ArticleVersion> ArticleVersionRepository => this.articleVersionRepository;

        public IReadOnlyRepository<Language> LanguageRepository => this.languageRepository;

        public IRepository<Page> PageRepository => this.pageRepository;

        public IRepository<PageVersion> PageVersionRepository => this.pageVersionRepository;

        public IRepository<PlainText> PlainTextRepository => this.plainTextRepository;

        public IRepository<RichText> RichTextRepository => this.richTextRepository;

        public IRepository<User> UserRepository => this.userRepository;

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
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
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
