namespace MVC.Core.Testing
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Entities.Account;
    using Entities.Culture;
    using Entities.Website;
    using Entities.Website.PageItem;
    using Entities.Article;
    using Entities.Location;

    public class MockUnitOfWork : IUnitOfWork
	{
		private readonly IRepository<Article, int> articleRepository;
        private readonly IRepository<ArticleVersion, int> articleVersionRepository;
        private readonly IRepository<Country, string> countryRepository;
        private readonly IRepository<Language, string> languageRepository;
        private readonly IRepository<Page, int> pageRepository;
        private readonly IRepository<PageVersion, int> pageVersionRepository;
        private readonly IRepository<PlainText, int> plainTextRepository;
        private readonly IRepository<RichText, int> richTextRepository;
        private readonly IRepository<User, int> userRepository;

        public MockUnitOfWork()
		{
			this.articleRepository = new MockRepository<Article, int>();
            this.articleVersionRepository = new MockRepository<ArticleVersion, int>();
            this.countryRepository = new MockRepository<Country, string>();
            this.languageRepository = new MockRepository<Language, string>();
            this.pageRepository = new MockRepository<Page, int>();
            this.pageVersionRepository = new MockRepository<PageVersion, int>();
            this.plainTextRepository = new MockRepository<PlainText, int>();
            this.richTextRepository = new MockRepository<RichText, int>();
            this.userRepository = new MockRepository<User, int>();
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
		}

#pragma warning disable CS1998 // ignore 'async lacks await' errors
        public async Task CommitAsync()
		{
		}
#pragma warning restore CS1998

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
