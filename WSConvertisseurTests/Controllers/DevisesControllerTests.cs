using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSConvertisseur.Controllers;
using WSConvertisseur.Models;

namespace WSConvertisseur.Controllers.Tests
{
    [TestClass()]
    public class DevisesControllerTests
    {
        #region GetAll
        [TestMethod()]
        public void GetAll_ReturnsAllItems()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            List<Devise> devisesAttendues = new List<Devise>
            {
                new Devise(1, "Dollar", 1.08),
                new Devise(2, "Franc Suisse", 1.07),
                new Devise(3, "Yen", 120)
            };

            // Act
            var result = controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Devise>), "Pas un IEnumerable<Devise>");
            CollectionAssert.AreEqual(devisesAttendues, result.ToList(), "Listes de devises pas identiques");
        }
        #endregion

        #region GetById
        [TestMethod()]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de retour
            Assert.IsNull(result.Result, "Erreur est pas null"); //Test de l'erreur
            Assert.IsInstanceOfType(result.Value, typeof(Devise), "Pas une Devise"); // Test du type du contenu (valeur) du retour
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), (Devise?)result.Value, "Devises pas identiques"); //Test de la devise récupérée
        }

        [TestMethod()]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetById(20);

            // Assert
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
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(4, "Rouble", 89.5);

            // Act
            var result = controller.Post(devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtRouteResult), "Pas un CreatedAtRouteResult");
            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result!;
            Assert.AreEqual(StatusCodes.Status201Created, routeResult.StatusCode, "Pas 201");
            Assert.AreEqual(devise, (Devise?)routeResult.Value, "Devises pas identiques");
        }

        // Test non fonctionnel : la validation du modèle ([Required]) n'est réalisée qu'à l'exécution de l'API.
        // Un test unitaire ne peut donc pas tester un modèle non valide.
        //[TestMethod()]
        //public void Post_InvalidObjectPassed_ReturnsBadRequest()
        //{
        //    // Arrange
        //    DevisesController controller = new DevisesController();
        //    Devise devise = new Devise(4, null, 4.0);
        //
        //    // Act
        //    var result = controller.Post(devise);
        //
        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
        //    Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult), "Pas un BadRequestObjectResult");
        //}
        #endregion

        #region Put
        [TestMethod()]
        public void Put_InvalidIdPassed_ReturnsBadRequest()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(2, "Franc Suisse", 1.10);

            // Act
            var result = controller.Put(1, devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "Pas un BadRequestResult");
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestResult)result).StatusCode, "Pas 400");
        }

        [TestMethod()]
        public void Put_UnknownIdPassed_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(20, "Livre Sterling", 0.87);

            // Act
            var result = controller.Put(20, devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.AreEqual(StatusCodes.Status404NotFound, ((NotFoundResult)result).StatusCode, "Pas 404");
        }

        [TestMethod()]
        public void Put_ValidObjectPassed_ReturnsNoContent()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(1, "Dollar", 1.15);

            // Act
            var result = controller.Put(1, devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.AreEqual(StatusCodes.Status204NoContent, ((NoContentResult)result).StatusCode, "Pas 204");
            Assert.AreEqual(devise, controller.GetById(1).Value, "Devise pas mise à jour");
        }
        #endregion

        #region Delete
        [TestMethod()]
        public void Delete_ExistingIdPassed_ReturnsDeletedItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsNull(result.Result, "Erreur est pas null");
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), result.Value, "Devises pas identiques");
            Assert.IsInstanceOfType(controller.GetById(1).Result, typeof(NotFoundResult), "Devise pas supprimée");
        }

        [TestMethod()]
        public void Delete_UnknownIdPassed_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.Delete(20);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.IsNull(result.Value, "Devise pas null");
        }
        #endregion
    }
}
