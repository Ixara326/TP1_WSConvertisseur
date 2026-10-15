using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSConvertisseur.Controllers;
using WSConvertisseur.Models;

namespace WSConvertisseur.Controllers.Tests
{
    [TestClass()]
    public class DevisesControllerTests
    {
        private DevisesController controller = null!;

        [TestInitialize]
        public void InitialisationDesTests()
        {
            controller = new DevisesController();
        }

        [TestCleanup]
        public void NettoyageDesTests()
        {
            controller = null!;
        }

        #region GetAll
        [TestMethod()]
        public void GetAll_ReturnsAllItems()
        {
            List<Devise> devisesAttendues = new List<Devise>
            {
                new Devise(1, "Dollar", 1.08),
                new Devise(2, "Franc Suisse", 1.07),
                new Devise(3, "Yen", 120)
            };

            var result = controller.GetAll();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<Devise>), "Pas un IEnumerable<Devise>");
            CollectionAssert.AreEqual(devisesAttendues, result.ToList(), "Listes de devises pas identiques");
        }
        #endregion

        #region GetById
        [TestMethod()]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            var result = controller.GetById(1);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult");
            Assert.IsNull(result.Result, "Erreur est pas null");
            Assert.IsInstanceOfType(result.Value, typeof(Devise), "Pas une Devise");
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), (Devise?)result.Value, "Devises pas identiques");
        }

        [TestMethod()]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            var result = controller.GetById(20);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult");
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.IsNull(result.Value, "Devise pas null");
            Assert.AreEqual(StatusCodes.Status404NotFound, ((NotFoundResult)result.Result!).StatusCode, "Pas 404");
        }
        #endregion

        #region Post
        [TestMethod()]
        public void Post_ValidObjectPassed_ReturnsCreatedAtRouteResult()
        {
            Devise devise = new Devise(4, "Rouble", 89.5);

            var result = controller.Post(devise);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtRouteResult), "Pas un CreatedAtRouteResult");
            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result!;
            Assert.AreEqual(StatusCodes.Status201Created, routeResult.StatusCode, "Pas 201");
            Assert.AreEqual(devise, (Devise?)routeResult.Value, "Devises pas identiques");
        }

        //[TestMethod()]
        //public void Post_InvalidObjectPassed_ReturnsBadRequest()
        //{
        //    Devise devise = new Devise(4, null, 4.0);
        //    var result = controller.Post(devise);
        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
        //    Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult), "Pas un BadRequestObjectResult");
        //}
        #endregion

        #region Put
        [TestMethod()]
        public void Put_InvalidIdPassed_ReturnsBadRequest()
        {
            Devise devise = new Devise(2, "Franc Suisse", 1.10);

            var result = controller.Put(1, devise);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "Pas un BadRequestResult");
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestResult)result).StatusCode, "Pas 400");
        }

        [TestMethod()]
        public void Put_UnknownIdPassed_ReturnsNotFound()
        {
            Devise devise = new Devise(20, "Livre Sterling", 0.87);

            var result = controller.Put(20, devise);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.AreEqual(StatusCodes.Status404NotFound, ((NotFoundResult)result).StatusCode, "Pas 404");
        }

        [TestMethod()]
        public void Put_ValidObjectPassed_ReturnsNoContent()
        {
            Devise devise = new Devise(1, "Dollar", 1.15);

            var result = controller.Put(1, devise);

            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.AreEqual(StatusCodes.Status204NoContent, ((NoContentResult)result).StatusCode, "Pas 204");
            Assert.AreEqual(devise, controller.GetById(1).Value, "Devise pas mise à jour");
        }
        #endregion

        #region Delete
        [TestMethod()]
        public void Delete_ExistingIdPassed_ReturnsDeletedItem()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsNull(result.Result, "Erreur est pas null");
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), result.Value, "Devises pas identiques");
            Assert.IsInstanceOfType(controller.GetById(1).Result, typeof(NotFoundResult), "Devise pas supprimée");
        }

        [TestMethod()]
        public void Delete_UnknownIdPassed_ReturnsNotFound()
        {
            var result = controller.Delete(20);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.IsNull(result.Value, "Devise pas null");
        }
        #endregion
    }
}
