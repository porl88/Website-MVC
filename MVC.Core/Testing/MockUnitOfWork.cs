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

    public class MockUnitOfWork : IUnitOfWork
	{
		private readonly IRepository<Article> articleRepository;
        private readonly IRepository<ArticleVersion> articleVersionRepository;
        private readonly IRepository<Language> languageRepository;
        private readonly IRepository<Page> pageRepository;
        private readonly IRepository<PageVersion> pageVersionRepository;
        private readonly IRepository<PlainText> plainTextRepository;
        private readonly IRepository<RichText> richTextRepository;
        private readonly IRepository<User> userRepository;

        public MockUnitOfWork()
		{
			this.articleRepository = new MockRepository<Article>();
            this.articleVersionRepository = new MockRepository<ArticleVersion>();
            this.languageRepository = new MockRepository<Language>();
            this.pageRepository = new MockRepository<Page>();
            this.pageVersionRepository = new MockRepository<PageVersion>();
            this.plainTextRepository = new MockRepository<PlainText>();
            this.richTextRepository = new MockRepository<RichText>();
            this.userRepository = new MockRepository<User>();
        }

        public IRepository<Article> ArticleRepository => this.articleRepository;

        public IRepository<ArticleVersion> ArticleVersionRepository => this.articleVersionRepository;

        public IRepository<Language> LanguageRepository => this.languageRepository;

        public IRepository<Page> PageRepository => this.pageRepository;

        public IRepository<PageVersion> PageVersionRepository => this.pageVersionRepository;

        public IRepository<PlainText> PlainTextRepository => this.plainTextRepository;

        public IRepository<RichText> RichTextRepository => this.richTextRepository;

        public IRepository<User> UserRepository => this.userRepository;

        public void Commit()
		{
		}

		public async Task CommitAsync()
		{
		}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
