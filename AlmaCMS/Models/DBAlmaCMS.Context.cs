﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlmaCMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_AlmaCmsEntities : DbContext
    {
        public DB_AlmaCmsEntities()
            : base("name=DB_AlmaCmsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<ArticleGroup> ArticleGroups { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<CerticicateGroup> CerticicateGroups { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<LinksGroup> LinksGroups { get; set; }
        public virtual DbSet<MainSlider> MainSliders { get; set; }
        public virtual DbSet<MediaType> MediaTypes { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsGroup> NewsGroups { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<ProductsGroup> ProductsGroups { get; set; }
        public virtual DbSet<ProductsImage> ProductsImages { get; set; }
        public virtual DbSet<ProjecGroup> ProjecGroups { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Retailer> Retailers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RssReader> RssReaders { get; set; }
        public virtual DbSet<SecureArticle> SecureArticles { get; set; }
        public virtual DbSet<SiteMessage> SiteMessages { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<SubPage> SubPages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ExpertInfo> ExpertInfoes { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<CustomeProsuctsImage> CustomeProsuctsImages { get; set; }
        public virtual DbSet<CustomProduct> CustomProducts { get; set; }
        public virtual DbSet<CustomeProductGroup> CustomeProductGroups { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Introduction> Introductions { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<ProductReview> ProductReviews { get; set; }
        public virtual DbSet<SendTime> SendTimes { get; set; }
        public virtual DbSet<SendType> SendTypes { get; set; }
        public virtual DbSet<VWPeyment> VWPeyments { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Favorit> Favorits { get; set; }
        public virtual DbSet<SiteSetting> SiteSettings { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<TaskDoc> TaskDocs { get; set; }
        public virtual DbSet<TaskExpert> TaskExperts { get; set; }
        public virtual DbSet<TasksList> TasksLists { get; set; }
        public virtual DbSet<TaskStatu> TaskStatus { get; set; }
        public virtual DbSet<VWTaskList> VWTaskLists { get; set; }
        public virtual DbSet<VWTaskUser> VWTaskUsers { get; set; }
        public virtual DbSet<VWExpertTask> VWExpertTasks { get; set; }
        public virtual DbSet<TaskAnswer> TaskAnswers { get; set; }
        public virtual DbSet<TaskAnwerDoc> TaskAnwerDocs { get; set; }
        public virtual DbSet<VWTaskAnswer> VWTaskAnswers { get; set; }
        public virtual DbSet<MessageAnswer> MessageAnswers { get; set; }
        public virtual DbSet<MessageExpert> MessageExperts { get; set; }
        public virtual DbSet<VWMessageUser> VWMessageUsers { get; set; }
        public virtual DbSet<VWMessageAnswer> VWMessageAnswers { get; set; }
        public virtual DbSet<ProductReportExpert> ProductReportExperts { get; set; }
        public virtual DbSet<RatilerType> RatilerTypes { get; set; }
        public virtual DbSet<ProductReportAnswerDoc> ProductReportAnswerDocs { get; set; }
        public virtual DbSet<ProductReportAnswer> ProductReportAnswers { get; set; }
        public virtual DbSet<ProductReportStatu> ProductReportStatus { get; set; }
        public virtual DbSet<ProductReport> ProductReports { get; set; }
        public virtual DbSet<ProductReportDoc> ProductReportDocs { get; set; }
        public virtual DbSet<VWProducReport> VWProducReports { get; set; }
        public virtual DbSet<VWProductReportExpert> VWProductReportExperts { get; set; }
        public virtual DbSet<VWProductReportAnswer> VWProductReportAnswers { get; set; }
        public virtual DbSet<VWExpertReport> VWExpertReports { get; set; }
        public virtual DbSet<BagTransaction> BagTransactions { get; set; }
        public virtual DbSet<DiscountCode> DiscountCodes { get; set; }
        public virtual DbSet<DiscountHistory> DiscountHistories { get; set; }
        public virtual DbSet<UserBag> UserBags { get; set; }
        public virtual DbSet<AdminRole> AdminRoles { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<SendSM> SendSMS { get; set; }
        public virtual DbSet<UsersShortMessage> UsersShortMessages { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<UsersMessage> UsersMessages { get; set; }
        public virtual DbSet<ExpertSubUser> ExpertSubUsers { get; set; }
        public virtual DbSet<ProductComment> ProductComments { get; set; }
        public virtual DbSet<BirthDateGift> BirthDateGifts { get; set; }
        public virtual DbSet<VWUserInfo> VWUserInfoes { get; set; }
        public virtual DbSet<VWBagTransaction> VWBagTransactions { get; set; }
        public virtual DbSet<TempUserLogin> TempUserLogins { get; set; }
    }
}
