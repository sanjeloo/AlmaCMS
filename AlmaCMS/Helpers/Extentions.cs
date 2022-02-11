using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using System.Linq.Expressions;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace AlmaCMS.Helpers
{
    public static class Extentions
    {

        public static bool HAsPermission(string _permission)
        {
            DB_AlmaCmsEntities db = new DB_AlmaCmsEntities();
            var UserId = HttpContext.Current.User.Identity.GetUserId();
            var CurrentUser = db.AspNetUsers.Where(c => c.Id == UserId).FirstOrDefault();
            var CurrentRole = db.Roles.Where(c => c.RoleName == _permission).FirstOrDefault();
            var userPermission = db.AdminRoles.Where(c => c.RoleId == CurrentRole.Id && c.UserId == CurrentUser.Id).ToList();
            if (userPermission.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #region Product Review

        public static VMProductReview toVMpRODUCTReviews(this ProductReview producreview)
        {
            return new VMProductReview()
            {
                ProductID = (int)producreview.productid,
                Comment = producreview.Comment,
                DateInsert = (DateTime)producreview.DateInsert,
                Id = producreview.Id,
                IP = producreview.IP,
                Name = producreview.Name,
                Rate = (int)producreview.Rate,
                Status = (bool)producreview.Status,
                Title = producreview.Title,
                Email = producreview.Email,
                Answer = producreview.Answer,


            };
        }

        public static ProductReview toProductReview(this VMProductReview vmproductreview)
        {
            return new ProductReview()
            {
                Title = vmproductreview.Title,
                Status = vmproductreview.Status,
                Rate = vmproductreview.Rate,
                Name = vmproductreview.Name,
                IP = vmproductreview.IP,
                Id = vmproductreview.Id,
                Email = vmproductreview.Email,
                DateInsert = vmproductreview.DateInsert,
                Comment = vmproductreview.Comment,
                productid = vmproductreview.ProductID,
                Answer = vmproductreview.Answer,

            };
        }


        #endregion

        public static bool HasAcceas(string RoleName, string UserId)
        {
            DB_AlmaCmsEntities db = new DB_AlmaCmsEntities();
            var CurrentUser = db.AspNetUsers.Where(c => c.Id == UserId).FirstOrDefault();
            var CurrentRole = db.Roles.Where(c => c.RoleName == RoleName).FirstOrDefault();
            var userPermission = db.AdminRoles.Where(c => c.RoleId == CurrentRole.Id && c.UserId == CurrentUser.Id).ToList();
            if (userPermission.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region DiscountCode Extention

        public static VMDiscountCode toVMDiscountCode(this DiscountCode DiscountCode)
        {
            return new VMDiscountCode()
            {
                id = DiscountCode.id,
               DiscountPrice=(long)DiscountCode.Discount_price,




                strCode=DiscountCode.Code,
                 Descriptions=DiscountCode.Descriptions,
                  Used=(bool)DiscountCode.Used,
                   CreateDate=(DateTime)DiscountCode.CreateDate,
                    ExpireDate=(DateTime)DiscountCode.ExpireDate,
                     
                  
            };

        }

        public static DiscountCode toDiscountCode(this VMDiscountCode vmDiscountCode)
        {
            return new DiscountCode()
            {
                id = vmDiscountCode.id,
                Discount_price = (long)vmDiscountCode.DiscountPrice,




                Code = vmDiscountCode.strCode,
                Descriptions = vmDiscountCode.Descriptions,
                Used = (bool)vmDiscountCode.Used,
                CreateDate = (DateTime)vmDiscountCode.CreateDate,
                ExpireDate = (DateTime)vmDiscountCode.ExpireDate,

            };
        }
        #endregion

        #region City Extention

        public static VMCity toVMCity(this City city)
        {
            return new VMCity()
            {
                Id = city.Id,
                Title = city.Title,
                Comments = city.Comments,
                StateID = (int)city.StateID,
            };

        }

        public static City toCity(this VMCity vmcity)
        {
            return new City()
            {
                Id = vmcity.Id,
                Title = vmcity.Title,
                Comments = vmcity.Comments,
                StateID = (int)vmcity.StateID,

            };
        }
        #endregion
        #region UserInfo Extension
        public static VMUserInfo toVMUserInfo(this UserInfo userinfo)
        {
            return new VMUserInfo()
            {
                id = userinfo.id,
                Address = userinfo.Address,
                BirthDate = (DateTime)userinfo.BirthDate,
                Name = userinfo.Name,
                Phone = userinfo.Phone,
                UserId = userinfo.UserId,
                Mobile = userinfo.Mobile,
                CodeMelli = userinfo.CodeMelli,
                 City=userinfo.City,
                  IntroductionTypeId=(int)userinfo.IntroductionTypeId,
                   Tel=userinfo.Tel,
                    Stateid=(int)userinfo.SatetId,
                     PostalCode=userinfo.PostalCode
            };
        }

        public static UserInfo toUserInfo(this VMUserInfo vmUserInfo)
        {
            return new UserInfo()
            {
                id = vmUserInfo.id,
                Address = vmUserInfo.Address,
                BirthDate = vmUserInfo.BirthDate,
                Name = vmUserInfo.Name,
                Phone = vmUserInfo.Phone,
                UserId = vmUserInfo.UserId,
                CodeMelli = vmUserInfo.CodeMelli,
                Mobile = vmUserInfo.Mobile,
                 City=vmUserInfo.City,
                  IntroductionTypeId=vmUserInfo.IntroductionTypeId,
                   PostalCode=vmUserInfo.PostalCode,
                    SatetId=vmUserInfo.Stateid,
                     Tel=vmUserInfo.Tel
            };
        }
        #endregion

        #region State Extention

        public static VMState toVMState(this State state)
        {
            return new VMState()
            {
                id = state.id,
                Title = state.Title,
                Comments = state.Comments,
            };

        }

        public static State toState(this VMState vmstate)
        {
            return new State()
            {
                id = vmstate.id,
                Title = vmstate.Title,
                Comments = vmstate.Comments,

            };
        }
        #endregion
        #region CustomeProducts extensions
        public static VMCustomeProduct toVMCustomeProduct(this Models.CustomProduct customeproduct)
        {
            return new VMCustomeProduct()
            {
                Descriptions = customeproduct.Descriptions,
                id = customeproduct.id,
                Image = customeproduct.Image,
                Keywords = customeproduct.Keywords,

                Title = customeproduct.Title,
                GroupID = (int)customeproduct.GroupId,
                PageContent = customeproduct.PageContent,
                 
            };
        }

        public static CustomProduct tocustomePtoduct(this VMCustomeProduct vmcustomeProducts)
        {
            return new CustomProduct()
            {
                Descriptions = vmcustomeProducts.Descriptions,
                id = vmcustomeProducts.id,
                Image = vmcustomeProducts.Image,
                Keywords = vmcustomeProducts.Keywords,

                Title = vmcustomeProducts.Title,
                GroupId = (int)vmcustomeProducts.GroupID,
                PageContent = vmcustomeProducts.PageContent,
              

            };
        }

        #endregion

        #region CustomeProductsImage extensions
        public static VMcustomeProductImage TovmCustomeProductsImage(this CustomeProsuctsImage CustomeproductsImage)
        {

            return new VMcustomeProductImage()
            {
                id = CustomeproductsImage.id,
                Image = CustomeproductsImage.Image,

                ProductID = (int)CustomeproductsImage.ProductID,
                Title = CustomeproductsImage.Title,


            };
        }

        public static CustomeProsuctsImage ToCustomeProductsImage(this VMcustomeProductImage vmCustomeProductsImage)
        {
            return new CustomeProsuctsImage()
            {
                id = vmCustomeProductsImage.id,
                Image = vmCustomeProductsImage.Image,
                ProductID = vmCustomeProductsImage.ProductID,
                Title = vmCustomeProductsImage.Title,

            };
        }
        #endregion

        public static string toSlugify(this string strUrl)
        {
            return strUrl.Replace(" ", "-").Replace(".", "").Replace("?", "").Replace("؟", "").Replace("/", "").Replace("%", "").Replace("\\", "");
        }
        #region Slider Extention

        public static VMSlider toVMSlider(this Slider slider)
        {
            return new VMSlider()
            {
                id = slider.id,
                Title = slider.Title,
                Description = slider.Description,
                ActionUrl = slider.ActionUrl,
                Image = slider.Image,
                Priority = (int)slider.Priority,
                Active = (bool)slider.Active,
            };

        }

        public static Slider toSlider(this VMSlider vmslider)
        {
            return new Slider()
            {
                id = vmslider.id,
                Title = vmslider.Title,
                Description = vmslider.Description,
                ActionUrl = vmslider.ActionUrl,
                Image = vmslider.Image,
                Priority = vmslider.Priority,
                Active = vmslider.Active,

            };
        }
        #endregion
        #region ProductsImage extensions
        public static VMProductsImage TovmProductsImage(this ProductsImage productsImage)
        {

            return new VMProductsImage()
            {
                id = productsImage.id,
                Image = productsImage.Image,

                ProductID = (int)productsImage.ProductID,
                Title = productsImage.Title,


            };
        }

        public static ProductsImage ToProductsImage(this VMProductsImage vmProductsImage)
        {
            return new ProductsImage()
            {
                id = vmProductsImage.id,
                Image = vmProductsImage.Image,
                ProductID = vmProductsImage.ProductID,
                Title = vmProductsImage.Title,

            };
        }
        #endregion
        #region Menu Items extensions
        public static VMMenuItems toVMMenuItems(this MenuItem menuitem)
        {
            return new VMMenuItems()
            {
                Comments = menuitem.Comments,
                DirectUrl = menuitem.DirectUrl,
                id = menuitem.id,
                IsPrimary = (bool)menuitem.IsPrimary,
                Name = menuitem.Name,
                OpenInNew = (bool)menuitem.OpenInNew,
                ParentID = (int)menuitem.ParentID,
                Title = menuitem.Title,
                Priority = (int)menuitem.Priority


            };
        }

        public static MenuItem toMenuItem(this VMMenuItems vmMenuItem)
        {
            return new MenuItem()
            {
                Comments = vmMenuItem.Comments,
                DirectUrl = vmMenuItem.DirectUrl,
                id = vmMenuItem.id,
                IsPrimary = (bool)vmMenuItem.IsPrimary,
                Name = vmMenuItem.Name,
                OpenInNew = (bool)vmMenuItem.OpenInNew,
                ParentID = (int)vmMenuItem.ParentID,
                Title = vmMenuItem.Title,
                Priority=(int)vmMenuItem.Priority

            };
        }
        #endregion

        #region subPages extensions
        public static VMSubpage toVMSubPages(this SubPage subpage)
        {
            return new VMSubpage()
            {
                id = subpage.id,
                PageId = (int)subpage.PageId,
                Image = subpage.Image,
                PageContent = subpage.PageContent,
                Priority = (int)subpage.Priority,
                ShortDescriptions = subpage.ShortDescriptions,
                Title = subpage.ShortDescriptions

            };
        }

        public static SubPage tosubPage(this VMSubpage vmSubPages)
        {
            return new SubPage()
            {
                 id=vmSubPages.id,
                  PageId=(int)vmSubPages.PageId,
                   Image=vmSubPages.Image,
                    PageContent=vmSubPages.PageContent,
                     Priority=(int)vmSubPages.Priority,
                      ShortDescriptions=vmSubPages.ShortDescriptions,
                       Title=vmSubPages.ShortDescriptions
            };
        }
        #endregion

        #region Pages extensions
        public static VMPages toVMPages(this Page page)
        {
            return new VMPages()
            {
                DirectUrl = page.DirectLink,
                ImageDesc = page.ImageDesc,
                dateInsert = (DateTime)page.DateInsert,
                ImageTitle = page.ImageTitle,
                ImageUrl = page.ImageUrl,
                KeyWords = page.Keywords,
                PageContent = page.PageContent,
                PageID = page.PegeID,
                PageName = page.PageName,
                ParentId = (int)page.ParentID,
                ShortDesc = page.ShortDesc,
                ShowInMenu = (bool)page.ShowInMenu,
                Title = page.Title,
                UniqID = page.UniqID,
                UserID = (int)page.UesrID

            };
        }

        public static Page toPage(this VMPages vmPages)
        {
            return new Page()
            {
                DirectLink = vmPages.DirectUrl,
                ImageDesc = vmPages.ImageDesc,
                DateInsert = (DateTime)vmPages.dateInsert,
                ImageTitle = vmPages.ImageTitle,
                ImageUrl = vmPages.ImageUrl,
                Keywords = vmPages.KeyWords,
                PageContent = vmPages.PageContent,
                PegeID = vmPages.PageID,
                PageName = vmPages.PageName,
                ParentID = (int)vmPages.ParentId,
                ShortDesc = vmPages.ShortDesc,
                ShowInMenu = (bool)vmPages.ShowInMenu,
                Title = vmPages.Title,
                UniqID = vmPages.UniqID,
                UesrID = (int)vmPages.UserID

            };
        }
        #endregion

        #region NewsGroup extensions
        public static VMNewsGroup toVMNewsGroup(this NewsGroup newsgroup)
        {
            return new VMNewsGroup()
            {
                id = newsgroup.id,
                Comments = newsgroup.Comments,
                Keywords = newsgroup.Keywords,
                Title = newsgroup.Title
            };
        }

        public static NewsGroup toNewsGroup(this VMNewsGroup vmNewsGroup)
        {
            return new NewsGroup()
            {
                Comments = vmNewsGroup.Comments,
                id = vmNewsGroup.id,
                Keywords = vmNewsGroup.Keywords,
                Title = vmNewsGroup.Title

            };
        }
        #endregion

        #region News extensions
        public static VMNews ToVmNews(this News news)
        {
            var pc = new System.Globalization.PersianCalendar();
            return new VMNews()
            {
                id = news.id,
                NewsGroupID = (int)news.NewsGroupID
                ,
                Active = (bool)news.Active,
                DateInsert = (DateTime)news.DateInsert,
                Image = news.Image,
                Keyword = news.Keyword,
                ShortDesc = news.ShortDesc,
                NewsContent = news.NewsContent,
                Title = news.Title,
                persianDate = pc.GetYear((DateTime)news.DateInsert) + "/" + pc.GetMonth((DateTime)news.DateInsert) + "/" + pc.GetDayOfMonth((DateTime)news.DateInsert)
                ,VisitCount=(int)news.VisitCount
            };
        }

        public static News ToNews(this VMNews vmNews)
        {
            return new News()
            {
                id = vmNews.id,
                Active = vmNews.Active,
                DateInsert = vmNews.DateInsert,
                Image = vmNews.Image,
                Keyword = vmNews.Keyword,
                ShortDesc = vmNews.ShortDesc,
                NewsContent = vmNews.NewsContent,
                Title = vmNews.Title,
                NewsGroupID = vmNews.NewsGroupID,
                 VisitCount=vmNews.VisitCount
            };
        }
        #endregion
        #region Article Group extensions
        public static VMArticleGroup toVMArticleGroup(this ArticleGroup articleGroup)
        {
            return new VMArticleGroup()
            {
                id = articleGroup.id,
                Comments = articleGroup.Comments,
                Keywords = articleGroup.Keywords,
                Title = articleGroup.Title
            };
        }

        public static ArticleGroup toArticleGroup(this VMArticleGroup vmArticleGroup)
        {
            return new ArticleGroup()
            {
                Comments = vmArticleGroup.Comments,
                id = vmArticleGroup.id,
                Keywords = vmArticleGroup.Keywords,
                Title = vmArticleGroup.Title

            };
        }
        #endregion

        #region Article extensions
        public static VMArticle toVMArticle(this Article artocle)
        {
            var pc = new System.Globalization.PersianCalendar();
            return new VMArticle()
            {
                id = artocle.id,
                ArticleGroupID = (int)artocle.ArticleGroupID
                ,
                Active = (bool)artocle.Active,
                DateInsert = (DateTime)artocle.DateInsert,
                Image = artocle.Image,
                Keyword = artocle.Keyword,
                ShortDesc = artocle.ShortDesc,
                ArticleContent = artocle.ArticleContent,
                Title = artocle.Title,
                persianDate = pc.GetYear((DateTime)artocle.DateInsert) + "/" + pc.GetMonth((DateTime)artocle.DateInsert) + "/" + pc.GetDayOfMonth((DateTime)artocle.DateInsert)
            };
        }

        public static Article toArticle(this VMArticle vmArticle)
        {
            return new Article()
            {
                id = vmArticle.id,
                Active = vmArticle.Active,
                DateInsert = vmArticle.DateInsert,
                Image = vmArticle.Image,
                Keyword = vmArticle.Keyword,
                ShortDesc = vmArticle.ShortDesc,
                ArticleContent = vmArticle.ArticleContent,
                Title = vmArticle.Title,
                ArticleGroupID = vmArticle.ArticleGroupID
            };
        }
        #endregion


        #region Article Group extensions
        public static VMAlbums toVMAlbume(this Album albume)
        {
            return new VMAlbums()
            {
                id = albume.id,
                Comments = albume.Comments,
                Keywords = albume.Keywords,
                Title = albume.Title
            };
        }

        public static Album toAlbume(this VMAlbums vmAlbume)
        {
            return new Album()
            {
                Comments = vmAlbume.Comments,
                id = vmAlbume.id,
                Keywords = vmAlbume.Keywords,
                Title = vmAlbume.Title

            };
        }
        #endregion

        #region Gallery extensions
        public static VMGallery ToVmGallery(this Gallery gallery)
        {
            return new VMGallery()
            {
                id = gallery.id,
                AlbumId = gallery.AlbumId,
                Comments = gallery.Comments,
                FileUrl = gallery.FileUrl,
                MediaType = (int)gallery.MediaType,
                TumbImage = gallery.TumbImage,
                MediaTypeTitle = gallery.MediaType1.Title
            };
        }

        public static Gallery ToGallery(this VMGallery vmGallery)
        {
            return new Gallery()
            {
                id = vmGallery.id,
                AlbumId = vmGallery.AlbumId,
                Comments = vmGallery.Comments,
                FileUrl = vmGallery.FileUrl,
                MediaType = (int)vmGallery.MediaType,
                TumbImage = vmGallery.TumbImage

            };
        }
        #endregion


        #region LinkGroup extensions
        public static VMLinkGroup tovmLinkGroup(this LinksGroup linkGroup)
        {
            return new VMLinkGroup()
            {
                Comments = linkGroup.Comments,
                id = linkGroup.id,
                Title = linkGroup.Title
            };
        }

        public static LinksGroup toLinkGroup(this VMLinkGroup vmLinkGroup)
        {
            return new LinksGroup()
            {
                Comments = vmLinkGroup.Comments,
                id = vmLinkGroup.id,
                Title = vmLinkGroup.Title
            };
        }
    
        #endregion

    #region Links extensions
        public static VMLinks   toVMlinks(this Link link)
        {
            return new VMLinks()
            {
                Active=(bool) link.Active,
                 id=link.id,
                  Image=link.Image,
                   LinkUrl=link.LinkUrl,
                    LinkGroupID=(int)link.LinkGroupID,
                     Title=link.Title,
                      Descriptions=link.DEscriptions
            };
        }

        public static Link toLink(this VMLinks vmLink)
        {
            return new Link()
            {
                Active = (bool)vmLink.Active,
                id = vmLink.id,
                Image = vmLink.Image,
                LinkUrl = vmLink.LinkUrl,
                LinkGroupID = (int)vmLink.LinkGroupID,
                Title = vmLink.Title,
                 DEscriptions=vmLink.Descriptions
            };
        }
    
        #endregion

    #region Question extensions
        public static VMQuestions   toVMQuestion(this Question question)
        {
            return new VMQuestions()
            {
                Active=(bool) question.Active,
                 id=question.id,
                   DateInsert=(DateTime)question.DateInsert,
                    Email=question.Email,
                     Title=question.Title,
                      AnswerText=question.AnswerText, 
                       Name=question.Name
            };
        }

        public static Question toQuestion(this VMQuestions vmQuestion)
        {
            return new Question()
            {
                Active = (bool)vmQuestion.Active,
                id = vmQuestion.id,
                 DateInsert=vmQuestion.DateInsert,
                  Email=vmQuestion.Email, 
                   Name=vmQuestion.Name,
                    Title=vmQuestion.Title,
                     AnswerText=vmQuestion.AnswerText
               
            };
        }
    
        #endregion

        #region Answer extensions
        public static VMAsnwer toVMAnswer(this Answer answer)
        {
            return new VMAsnwer()
            {
                AnswerText=answer.AnswerText,
                DateInsert=(DateTime)answer.DateInsert,
                 id=answer.id,
                  QuestionID=(int)answer.QuestionID
            };
        }

        public static Answer toAnswer(this VMAsnwer vmAnswer)
        {
            return new Answer()
            {
                AnswerText = vmAnswer.AnswerText,
                DateInsert = (DateTime)vmAnswer.DateInsert,
                id = vmAnswer.id,
                QuestionID = (int)vmAnswer.QuestionID

            };
        }

        #endregion


        #region ProductsGroup extensions
        public static VMProductGroup toVMProductGroup(this ProductsGroup productGroup)
        {
            return new VMProductGroup()
            {
                Description = productGroup.Description,
                 id=productGroup.id,
                  Image=productGroup.Image,
                   Keywords=productGroup.Keywords,
                    Priority=(int)productGroup.Priority,
                    ParentId = productGroup.ParentId,
                     Title=productGroup.Title
            };
        }

        public static ProductsGroup toProductsGroup(this VMProductGroup vmProductGroup)
        {
            return new ProductsGroup()
            {
                Description = vmProductGroup.Description,
                id = vmProductGroup.id,
                Image = vmProductGroup.Image,
                Keywords = vmProductGroup.Keywords,
                Priority = (int)vmProductGroup.Priority,
                Title = vmProductGroup.Title,
                ParentId = vmProductGroup.ParentId,

            };
        }

        #endregion

        #region Products extensions
        public static VMProducts toVMProduct(this Product product)
        {
            return new VMProducts()
            {
                Description = product.Description,
                id = product.id,
                Image = product.Image,
                Keywords = product.Keywords,
                Priority = (int)product.Priority,
                Title = product.Title,
                 GroupID=(int)product.GroupID,
                  Specification=product.Specification,
                Price = product.price ?? 0,
                PriceBeforeDiscount = product.PriceBeforeDiscount ?? 0,

                SpecialSaleStartDate = product.SpecialSaleStartDate,
                SpeciaSaleEndDate = product.SpeciaSaleEndDate,
                ExistStatus=(bool) product.ExistStatus,
                 ExistCount=(int)product.ExistCount,
                  VisitCount=product.VisitCount
            };
        }

        public static Product toPtoduct(this VMProducts vmProducts)
        {
            return new Product()
            {
                Description = vmProducts.Description,
                id = vmProducts.id,
                Image = vmProducts.Image,
                Keywords = vmProducts.Keywords,
                Priority = (int)vmProducts.Priority,
                Title = vmProducts.Title,
                GroupID = (int)vmProducts.GroupID,
                Specification = vmProducts.Specification,
                price = (long)vmProducts.Price ,
                PriceBeforeDiscount = vmProducts.PriceBeforeDiscount ,

                SpecialSaleStartDate = vmProducts.SpecialSaleStartDate,
                SpeciaSaleEndDate = vmProducts.SpeciaSaleEndDate,
                ExistStatus = (bool)vmProducts.ExistStatus,
                ExistCount = (int)vmProducts.ExistCount,
                 VisitCount=vmProducts.VisitCount
                 

            };
        }

        #endregion

        #region ProductsImage extensions
        public static VMProductImage toVMProductImage(this ProductsImage productimage)
        {
            return new VMProductImage()
            {

                id = productimage.id,
                Image = productimage.Image,

                Title = productimage.Title,
               
            };
        }

        public static ProductsImage toProductImage(this VMProductImage vmProductImage)
        {
            return new ProductsImage()
            {
                id = vmProductImage.id,
                Image = vmProductImage.Image,

                Title = vmProductImage.Title,

            };
        }

        #endregion

        #region Certificate Group extensions
        public static VMCertificateGroup toVMCertificateGroup(this CerticicateGroup certGroup)
        {
            return new VMCertificateGroup()
           {
               Descriotion = certGroup.Descriotion,
             
               id = certGroup.id,
               Image = certGroup.Image,
               Keywords = certGroup.Keywords,
         
               Title = certGroup.Title
           };
        }

        public static CerticicateGroup toCertificateGroup(this VMCertificateGroup vmCertificateGroup)
        {
            return new CerticicateGroup()
            {
                Descriotion = vmCertificateGroup.Descriotion,
      
                id = vmCertificateGroup.id,
                Image = vmCertificateGroup.Image,
                Keywords = vmCertificateGroup.Keywords,
    
                Title = vmCertificateGroup.Title

            };
        }

        #endregion




        #region Certificate extensions
        public static VMCertificates toVMCertificate(this Certificate certificate)
        {
            return new VMCertificates()
            {
                Descriotion = certificate.Descriptions,
                GroupID =(int) certificate.GroupID,
                id = certificate.id,
                Image = certificate.Image,
                Keywords = certificate.Keywords,
                Prority = (int)certificate.Prority,
                Title = certificate.Title
                
            };
        }

        public static Certificate toCertificate(this VMCertificates vmCertificate)
        {
            return new Certificate()
            {
                Descriptions = vmCertificate.Descriotion,
                GroupID = (int)vmCertificate.GroupID,
                id = vmCertificate.id,
                Image = vmCertificate.Image,
                Keywords = vmCertificate.Keywords,
                Prority = (int)vmCertificate.Prority,
                Title = vmCertificate.Title

            };
        }

        #endregion

        #region Project Group extensions
        public static VMprojectsgroup toVMProjectGroup(this ProjecGroup projectgroup)
        {
            return new VMprojectsgroup()
            {
                Descriotion = projectgroup.Descriotion,

                id = projectgroup.id,
                Image = projectgroup.Image,
                Keywords = projectgroup.Keywords,

                Title = projectgroup.Title
            };
        }

        public static ProjecGroup toProjectGroup(this VMprojectsgroup vmProjectHroup)
        {
            return new ProjecGroup()
            {


                Descriotion = vmProjectHroup.Descriotion,

                id = vmProjectHroup.id,
                Image = vmProjectHroup.Image,
                Keywords = vmProjectHroup.Keywords,

                Title = vmProjectHroup.Title
            };
        }

        #endregion

        #region Projects extensions
        public static VMProjects toVMProject(this Project project)
        {
            return new VMProjects()
            {
                DateInsert = (DateTime)project.DateInsert,
                Description = project.Description,
                GroupID = (int)project.GroupID,
                id = project.id,
                Image =project.Image,
                Keywords = project.Keywords,
                Link = project.Link,
                ProjectContent = project.ProjectContent,
                Title = project.Title,
                Priority=(int)project.Priority

            };
        }

        public static Project toProject(this VMProjects vmproject)
        {
            return new Project()
            {
                DateInsert = (DateTime)vmproject.DateInsert,
                Description = vmproject.Description,
                GroupID = (int)vmproject.GroupID,
                id = vmproject.id,
                Image = vmproject.Image,
                Keywords = vmproject.Keywords,
                Link = vmproject.Link,
                ProjectContent = vmproject.ProjectContent,
                Title = vmproject.Title,
                Priority=(int)vmproject.Priority

            };
        }

        #endregion
        #region RssReader extensions
        public static VMRSSReader toVMRssReader(this RssReader rssreader)
        {
            return new VMRSSReader()
            {
                FeedID = rssreader.FeedID,
                Title = rssreader.Title,
                Url = rssreader.Url

            };
        }

        public static RssReader toRssReader(this VMRSSReader vmressreader)
        {
            return new RssReader()
            {
                FeedID = vmressreader.FeedID,
                Title = vmressreader.Title,
                Url = vmressreader.Url

            };
        }

        #endregion

        #region Ratilers extensions
        public static VMRetailers toVMRetailers(this Retailer retaile)
        {
            return new VMRetailers()
            {
               ActiveState=(bool)retaile.ActiveState,
                Address=retaile.Address,
                 Comments=retaile.Comments,
                  CompanyName=retaile.CompanyName,
                   Email=retaile.Email,
                   id=retaile.id,
                    Mobile=retaile.Mobile,
                     Name=retaile.Name,
                      Pass=retaile.Pass,
                        Tel=retaile.Tel,
                         USerName=retaile.USerName,
                          Website=retaile.Website
                    

            };
        }

        public static Retailer toRetailer(this VMRetailers vmretailers)
        {
            return new Retailer()
            {
                ActiveState = (bool)vmretailers.ActiveState,
                Address = vmretailers.Address,
                Comments = vmretailers.Comments,
                CompanyName = vmretailers.CompanyName,
                Email = vmretailers.Email,
                id = vmretailers.id,
                Mobile = vmretailers.Mobile,
                Name = vmretailers.Name,
                Pass = vmretailers.Pass,
                Tel = vmretailers.Tel,
                USerName = vmretailers.USerName,
                Website = vmretailers.Website
                    

            };
        }

        #endregion

        #region Secure Articles extensions
        public static VMSecureArticle toVMScureArticles(this SecureArticle securearticle)
        {
            return new VMSecureArticle()
            {
                ArticleContent=securearticle.ArticleContent,
                 DateInsert=(DateTime)securearticle.DateInsert,
                  id=securearticle.id,
                   Image=securearticle.image,
                    Title= securearticle.Title


            };
        }

        public static SecureArticle toRetoSecureArticle(this VMSecureArticle vmsecurearticle  )
        {
            return new SecureArticle()
            {
                ArticleContent = vmsecurearticle.ArticleContent,
                DateInsert = (DateTime)vmsecurearticle.DateInsert,
                id = vmsecurearticle.id,
                image = vmsecurearticle.Image,
                Title = vmsecurearticle.Title


            };
        }

        #endregion

        #region Persian dateTime extensions
        public static string ToPersianShortDate(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(dateTime).ToString();
            string m = pc.GetMonth(dateTime).ToString().PadLeft(2, '0');
            string d = pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0');

            return string.Format("{0}/{1}/{2}", y, m, d);
        }
        #endregion

        #region SiteSetting
        public static VMSiteSettings toVMSiteSettings(this SiteSetting sitesetting)
        {
            return new VMSiteSettings()
            {
                Id = sitesetting.Id,
                Title = sitesetting.Title,
                SiteIcon = sitesetting.SiteIcone,
                MetaKeywords = sitesetting.MetaKeyWords,
                MetaDescription = sitesetting.MetaDescription,
                MailUser = sitesetting.MailUser,
                MailForm = sitesetting.MailForm,
                MailPassWord = sitesetting.MailPassword,
                MailPort = sitesetting.MailPort,
                MailSMTP = sitesetting.MailSMPT,
                Disclaimer = sitesetting.Disclaimer,
                FooterAbout = sitesetting.FooterAbout,
                FooterText = sitesetting.FooterText,
                FactorActive = (bool)sitesetting.FactorActive,
                VAT = (bool)sitesetting.VAT,
                VATPercent = (double)sitesetting.VatPercent,
                SendBoxingPrice = (double)sitesetting.SendBoxingPrice,
                SendInsurancePrice = (double)sitesetting.SendInsurancePrice,
                SendTaxPrice = (double)sitesetting.SendTaxPrice,
                SentVatPrice = (double)sitesetting.SentVatPrice, 

                 BirtDateGift=(bool)sitesetting.SendBirthDateGift,
                  BirtDateGiftPrice=(long) sitesetting.BirthDateGiftPrice,
                   ProfitPercent=(int)sitesetting.ProfitPercent,
                    ProfitPercentCount=(int)sitesetting.ProfitPercentCount


            };
        }
        public static SiteSetting toSiteSetting(this VMSiteSettings vmsitesetting)
        {
            return new SiteSetting()
            {
                Id = vmsitesetting.Id,
                Title = vmsitesetting.Title,
                SiteIcone = vmsitesetting.SiteIcon,
                MetaKeyWords = vmsitesetting.MetaKeywords,
                MetaDescription = vmsitesetting.MetaDescription,
                MailUser = vmsitesetting.MailUser,
                MailForm = vmsitesetting.MailForm,
                MailPassword = vmsitesetting.MailPassWord,
                MailPort = vmsitesetting.MailPort,
                MailSMPT = vmsitesetting.MailSMTP,
                FooterText = vmsitesetting.FooterText,
                FooterAbout = vmsitesetting.FooterAbout,
                Disclaimer = vmsitesetting.Disclaimer,
                FactorActive = vmsitesetting.FactorActive,
                VAT = vmsitesetting.VAT,
                VatPercent = (long)vmsitesetting.VATPercent,
                SendBoxingPrice = (long)vmsitesetting.SendBoxingPrice,
                SendInsurancePrice = (long)vmsitesetting.SendInsurancePrice,
                SendTaxPrice = (long)vmsitesetting.SendTaxPrice,
                SentVatPrice = (long)vmsitesetting.SentVatPrice,
                SendBirthDateGift = (bool)vmsitesetting.BirtDateGift,
                BirthDateGiftPrice = (long)vmsitesetting.BirtDateGiftPrice,
                 ProfitPercent=vmsitesetting.ProfitPercent,
                  ProfitPercentCount=vmsitesetting.ProfitPercentCount


            };
        }
        #endregion
}
}
