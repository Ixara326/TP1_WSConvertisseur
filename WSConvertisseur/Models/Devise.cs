using System.ComponentModel.DataAnnotations;

namespace WSConvertisseur.Models
{
    /// <summary>
    /// Classe métier représentant une devise et son taux de conversion à partir de l'euro.
    /// </summary>
    public class Devise
    {
        private int id;

        /// <summary>Identifiant de la devise</summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string? nomDevise;

        /// <summary>Nom de la devise (obligatoire)</summary>
        [Required]
        public string? NomDevise
        {
            get { return nomDevise; }
            set { nomDevise = value; }
        }

        private double taux;

        /// <summary>Taux de conversion (1 euro = Taux devise)</summary>
        public double Taux
        {
            get { return taux; }
            set { taux = value; }
        }

        /// <summary>Constructeur sans paramètre</summary>
        public Devise()
        {
        }

        /// <summary>Constructeur paramétré</summary>
        /// <param name="id">Identifiant</param>
        /// <param name="nomDevise">Nom de la devise</param>
        /// <param name="taux">Taux de conversion</param>
        public Devise(int id, string? nomDevise, double taux)
        {
            Id = id;
            NomDevise = nomDevise;
            Taux = taux;
        }
    }
}
