using CRMSTUBSOFT.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CRMSTUBSOFT.Services.Shared
{
    public class Frm_Impression
    {
        private String Titre, NomFichier, Condition = "", MontantLettre, CodeClient, CodeFournisseur, CodePersonnel,
        Arrendissement, DateDebut, DateFin, CodeDepot, Article, CodeArticle, Depot, ListeArticle, Etat,
        CodeCompte = "", NomCompte = "", NumeroTicket = "", Session = "", ConfigurationImprimente = "", Caissier = "";
        private int NBFois;
        private Int16 NumeroImprimante;
        bool ParametreXml = false;
        String Numcaisse = "", Nomutilisateur = "", DateJour = "";
        public Frm_Impression(String P_Titre, String P_NomFicher)
        {
            Titre = P_Titre;
            NomFichier = P_NomFicher;
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, int P_Num, String P_Session)
        {
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            Session = P_Session;
            NumeroImprimante = Convert.ToInt16(P_Num);
        }
        public Frm_Impression(String P_Titre, String P_NomFicher, int P_Num, String P_Session, String P_Utilisateur)
        {
            Caissier = P_Utilisateur;
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            Session = P_Session;
            NumeroImprimante = Convert.ToInt16(P_Num);
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, String Formule)
        {
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            if (Titre == "Liste Articles Vendu" || Titre == "Liste Articles Vendu   ")
            {
                Session = Formule;
            }
            else
            {
                Condition = Formule;
            }
        }
        public Frm_Impression(String P_Titre, String P_NomFicher, int P_NumeroImprimante, String P_Condition, Boolean XX)
        {
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            NumeroImprimante = Convert.ToInt16(P_NumeroImprimante);
            Condition = P_Condition;
        }

        public Frm_Impression(String P_Titre, String P_NomFchier, String P_Condition, String P_MontantLettre)
        {
            Titre = P_Titre;
            NomFichier = P_NomFchier;
            Condition = P_Condition;
            MontantLettre = P_MontantLettre;
        }

        public Frm_Impression(String P_Titre, String P_NomFchier, String P_Condition, String P_MontantLettre, Int16 P_NumeroImprimente, Boolean Const)
        {
            Titre = P_Titre;
            NomFichier = P_NomFchier;
            Condition = P_Condition;
            MontantLettre = P_MontantLettre;
            NumeroImprimante = P_NumeroImprimente;
        }

        public Frm_Impression(Boolean Cont, String P_Titre, String P_NomFchier, DateTime Datedeb, DateTime Datefi, String P_Personnel, Int16 P_NumeroImprimente)
        {
            Titre = P_Titre;
            NomFichier = P_NomFchier;
            DateDebut = Datedeb.ToShortDateString(); ;
            DateFin = Datefi.ToShortDateString();
            CodePersonnel = P_Personnel;
            NumeroImprimante = P_NumeroImprimente;
        }


        public Frm_Impression(String P_Titre, String P_NomFchier, String P_NumeroTicket, Int16 P_NumeroImprimante, String P_Type)
        {
            Titre = P_Titre;
            NomFichier = P_NomFchier;
            NumeroImprimante = P_NumeroImprimante;
            NumeroTicket = P_NumeroTicket;
            MontantLettre = P_Type;
            if (Titre == "Consomation")
            {
                Condition = "{BonLivraisonVente.NumeroBonLivraisonVente} ='" + NumeroTicket + "'";
            }
            if (Titre == "Ticket Pizza")
            {
                CodeArticle = P_Type;
                MontantLettre = "";
            }
            if (Titre == "Ticket Restaurant")
            {
                Condition = "{TempTicket.NumeroBonLivraisonVente} ='" + NumeroTicket + "'";
            }
            Frm_Impression_Load();
        }

        // constructeur pour parametre imprimente de fichier XML directement
        public Frm_Impression(bool P_ParametreXml, String P_Titre, String P_NomFchier, String P_NumeroTicket, String P_ConfigurationImprimente, String P_Type)
        {
            ParametreXml = P_ParametreXml;
            Titre = P_Titre;
            NomFichier = P_NomFchier;
            if (P_Titre == "Ticket")
            {
                NumeroImprimante = 2;
            }
            else
            {
                ConfigurationImprimente = P_ConfigurationImprimente;
            }
            NumeroTicket = P_NumeroTicket;
            MontantLettre = P_Type;
            if (Titre == "Consomation")
            {
                Condition = "{BonLivraisonVente.NumeroBonLivraisonVente} ='" + NumeroTicket + "'";
            }
        }


        public Frm_Impression(String P_Titre, String P_NomFchier, DateTime P_DateDebut)
        {

            Titre = P_Titre;
            NomFichier = P_NomFchier;
            DateDebut = P_DateDebut.ToString();
        }
        public Frm_Impression(String P_Titre, String P_NomFicher, String Formule, String P_DateDebut, String P_DateFin, int x, int y, int z)
        {
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            DateDebut = P_DateDebut;
            DateFin = P_DateFin;
            Condition = Formule;
        }

        public Frm_Impression(Boolean XX, Boolean YY, String P_Titre, String P_NomFchier, String P_Condition, String P_DateDebut, String P_DateFin, int Mode)
        {

            Titre = P_Titre;
            NomFichier = P_NomFchier;

            if (P_DateDebut != "")
            {
                DateDebut = P_DateDebut;
            }
            else
            {
                DateDebut = "";
            }
            if (P_DateFin != "")
            {
                DateFin = P_DateFin;
            }
            else
            {
                DateFin = DateTime.Today.ToShortDateString();
            }
            Condition = P_Condition;
        }

        public Frm_Impression(String P_Titre, String P_NomFchier, String P_DateDebut, String P_DateFin, int Mode)
        {

            Titre = P_Titre;
            NomFichier = P_NomFchier;

            if (P_DateDebut != "")
            {
                DateDebut = P_DateDebut;
            }
            else
            {
                DateDebut = "";
            }
            if (P_DateFin != "")
            {
                DateFin = P_DateFin;
            }
            else
            {
                DateFin = DateTime.Today.ToShortDateString();
            }
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, String P_CodeClient, String P_DateDebut, String P_DateFin)
        {
            if (Titre == "Rapport Caisse Globale")
            {
                Titre = P_Titre;
                NomFichier = P_NomFicher;

                DateJour = P_CodeClient;
                Numcaisse = P_DateDebut;
                Nomutilisateur = P_DateFin;
            }
            else
            {
                CodeClient = P_CodeClient;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                Titre = P_Titre;
                NomFichier = P_NomFicher;
            }
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, int r, String P_CodeCompte, String P_DateDebut, String P_DateFin, String P_NomCompte)
        {
            CodeCompte = P_CodeCompte;
            DateDebut = P_DateDebut;
            DateFin = P_DateFin;
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            NomCompte = P_NomCompte;
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, String P_CodeFournisseur, String P_DateDebut, String P_DateFin, String Fournisseur)
        {
            CodeFournisseur = P_CodeFournisseur;
            DateDebut = P_DateDebut;
            DateFin = P_DateFin;
            Titre = P_Titre;
            NomFichier = P_NomFicher;
        }

        public Frm_Impression(String P_Titre, String P_NomFicher, String P_CodeDepot, String P_Depot, String P_CodeArticle, String P_Article, String P_Arrendissement, String P_DateDebut, String P_DateFin)
        {
            CodeDepot = P_CodeDepot;
            Depot = P_Depot;
            CodeArticle = P_CodeArticle;
            Article = P_Article;
            Arrendissement = P_Arrendissement;
            DateDebut = P_DateDebut;
            DateFin = P_DateFin;
            Titre = P_Titre;
            NomFichier = P_NomFicher;

        }

        public Frm_Impression(String P_Titre, String P_NomFicher, String P_CodeDepot, String P_Depot, String P_Condition, int Value)
        {
            CodeDepot = P_CodeDepot;
            Depot = P_Depot;
            Titre = P_Titre;
            NomFichier = P_NomFicher;
            Condition = P_Condition;

        }



        //********* Impression des etats !!!! 
        public Frm_Impression(int Mode, String P_Titre, String P_NomFicher, String P_Tier, String P_DateDebut, String P_DateFin, String P_Etat, String TypeEtat)
        {

            if (TypeEtat == "BonCommandeFournisseur")
            {
                if (P_Etat == "En Cour")
                {
                    Condition = "{BonCommandeAchat.NumeroEtat} = 'E01'";
                }
                if (P_Etat == "Livrés")
                {
                    Condition = "{BonCommandeAchat.NumeroEtat} = 'E03'";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {BonCommandeAchat.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {BonCommandeAchat.DateBonCommandeAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        DateFin = DateTime.Today.ToShortDateString();
                        Condition = Condition + " AND {BonCommandeAchat.DateBonCommandeAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }

            if (TypeEtat == "BonLivraisonFournisseur")
            {

                if (P_Etat == "Non Facturés")
                {
                    Condition = "{BonLivraisonAchat.NumeroEtat} = 'E05' ";
                }
                if (P_Etat == "Facturés")
                {
                    Condition = "{BonLivraisonAchat.NumeroEtat} = 'E06' ";
                }
                if (P_Etat == "Payés")
                {
                    Condition = "{BonLivraisonAchat.NumeroEtat} = 'E08' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {BonLivraisonAchat.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {BonLivraisonAchat.DateBonLivraisonAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {BonLivraisonAchat.DateBonLivraisonAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }


            if (TypeEtat == "FactureFournisseur")
            {

                if (P_Etat == "Non Payés")
                {
                    Condition = "{FactureAchat.NumeroEtat} = 'E07' ";
                }
                if (P_Etat == "Payés")
                {
                    Condition = "{FactureAchat.NumeroEtat} = 'E08' ";
                }
                if (P_Etat == "")
                {
                    Condition = "1=1 ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {FactureAchat.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {FactureAchat.DateFactureAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {FactureAchat.DateFactureAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }

            if (TypeEtat == "AvoirFournisseur")
            {

                if (P_Etat == "non Remboursé")
                {
                    Condition = "{AvoirAchat.NumeroEtat} = 'E09' ";
                }
                if (P_Etat == "Remboursé")
                {
                    Condition = "{AvoirAchat.NumeroEtat} = 'E10' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {AvoirAchat.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {AvoirAchat.DateAvoirAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {AvoirAchat.DateAvoirAchat} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }


            }
            if (TypeEtat == "AvanceFournisseur")
            {

                if (P_Etat == "Non Appuré")
                {
                    Condition = "{Avancefournisseur.EtatAvance} = 'E11' ";
                }
                if (P_Etat == "Appuré")
                {
                    Condition = "{Avancefournisseur.EtatAvance} = 'E12' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {Avancefournisseur.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {Avancefournisseur.DateAvance} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {Avancefournisseur.DateAvance} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }

            if (TypeEtat == "ReglementFournisseur")
            {
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = " {VueListeReglementFournisseurEnCour.CodeFournisseur} ='" + CodeFournisseur + "' ";
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND  {VueListeReglementFournisseurEnCour.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin})   ";
                    }
                }
                else
                {
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + "  {VueListeReglementFournisseurEnCour.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin})   ";
                    }
                }


            }
            if (TypeEtat == "ReglementFournisseurPayee")
            {
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = " {VueListeReglementFournisseurPayee.CodeFournisseur} ='" + CodeFournisseur + "' ";
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " and {VueListeReglementFournisseurPayee.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }
                }
                else
                {
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " {VueListeReglementFournisseurPayee.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }
                }

            }

            if (TypeEtat == "PaymentFournisseur")
            {

                if (P_Etat == "En Cour")
                {
                    Condition = "{VueSuivieFournisseur.NumeroEtat} = 'E13' ";
                }
                if (P_Etat == "Versé")
                {
                    Condition = "{VueSuivieFournisseur.NumeroEtat} = 'E14' ";
                }
                if (P_Etat == "Impayé")
                {
                    Condition = "{VueSuivieFournisseur.NumeroEtat} = 'E15' ";
                }
                if (P_Etat == "Remplacé")
                {
                    Condition = "{VueSuivieFournisseur.NumeroEtat} = 'E16' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeFournisseur = P_Tier;
                Etat = P_Etat;
                if (CodeFournisseur != "")
                {
                    Condition = Condition + " AND {VueSuivieFournisseur.CodeFournisseur} ='" + CodeFournisseur + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {VueSuivieFournisseur.Echeance} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {VueSuivieFournisseur.Echeance} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                    }

                }

            }


            //************************ client *****************************************
            if (TypeEtat == "BonCommandeClient")
            {
                if (P_Etat == "En Cour")
                {
                    Condition = "{BonCommandeVente.NumeroEtat} = 'E01'";
                }
                if (P_Etat == "Livrés")
                {
                    Condition = "{BonCommandeVente.NumeroEtat} = 'E03'";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {BonCommandeVente.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {BonCommandeVente.DateBonCommandeVente} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {BonCommandeVente.DateBonCommandeVente} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                    }

                }

            }

            if (TypeEtat == "BonLivraisonClient")
            {

                if (P_Etat == "Non Facturés")
                {
                    Condition = "{BonLivraisonVente.NumeroEtat} = 'E05' ";
                }
                if (P_Etat == "Facturés")
                {
                    Condition = "{BonLivraisonVente.NumeroEtat} = 'E06' ";
                }
                if (P_Etat == "Payés")
                {
                    Condition = "{BonLivraisonVente.NumeroEtat} = 'E08' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {BonLivraisonVente.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {BonLivraisonVente.DateBonLivraisonVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {BonLivraisonVente.DateBonLivraisonVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }


            if (TypeEtat == "FactureClient")
            {

                if (P_Etat == "Non Payés")
                {
                    Condition = "{FactureVente.NumeroEtat} = 'E07' ";
                }
                if (P_Etat == "Payés")
                {
                    Condition = "{FactureVente.NumeroEtat} = 'E08' ";
                }
                if (P_Etat == "")
                {
                    Condition = "1=1";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {FactureVente.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {FactureVente.DateFactureVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {FactureVente.DateFactureVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }
                }

            }

            if (TypeEtat == "AvoirClient")
            {

                if (P_Etat == "non Remboursé")
                {
                    Condition = "{AvoirVente.NumeroEtat} = 'E09' ";
                }
                if (P_Etat == "Remboursé")
                {
                    Condition = "{AvoirVente.NumeroEtat} = 'E10' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {AvoirVente.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {AvoirVente.DateAvoirVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {AvoirVente.DateAvoirVente} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }


            }
            if (TypeEtat == "AvanceClient")
            {

                if (P_Etat == "Non Appuré")
                {
                    Condition = "{AvanceClient.EtatAvance} = 'E11' ";
                }
                if (P_Etat == "Appuré")
                {
                    Condition = "{AvanceClient.EtatAvance} = 'E12' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {AvanceClient.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {AvanceClient.DateAvance} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {AvanceClient.DateAvance} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }

                }

            }

            if (TypeEtat == "ReglementClient")
            {
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = " {VueListeReglementClientEnCour.CodeClient} ='" + CodeClient + "' ";
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND  {VueListeReglementClientEnCour.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin})   ";
                    }
                }
                else
                {
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " {VueListeReglementClientEnCour.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin})   ";
                    }
                }

            }
            if (TypeEtat == "ReglementClientPayee")
            {
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = " {VueListeReglementClientPayee.CodeClient} ='" + CodeClient + "' ";
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " and {VueListeReglementClientPayee.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }
                }
                else
                {
                    if ((DateDebut != "") && (DateFin != ""))
                    {
                        Condition = Condition + " {VueListeReglementClientPayee.DateReglement} IN CDate({?DateDebut}) TO CDate({?DateFin}) ";
                    }
                }

            }

            if (TypeEtat == "PaymentClient")
            {

                if (P_Etat == "En Cour")
                {
                    Condition = "{VueSuivieClient.NumeroEtat} = 'E13' ";
                }
                if (P_Etat == "Versé")
                {
                    Condition = "{VueSuivieClient.NumeroEtat} = 'E14' ";
                }
                if (P_Etat == "Impayé")
                {
                    Condition = "{VueSuivieClient.NumeroEtat} = 'E15' ";
                }
                if (P_Etat == "Remplacé")
                {
                    Condition = "{VueSuivieClient.NumeroEtat} = 'E16' ";
                }
                Titre = P_Titre;
                NomFichier = P_NomFicher;
                DateDebut = P_DateDebut;
                DateFin = P_DateFin;
                CodeClient = P_Tier;
                Etat = P_Etat;
                if (CodeClient != "")
                {
                    Condition = Condition + " AND {VueSuivieClient.CodeClient} ='" + CodeClient + "' ";

                }

                if ((DateDebut != "") && (DateFin != ""))
                {
                    Condition = Condition + " AND {VueSuivieClient.Echeance} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                }
                else
                {
                    if ((DateFin == "") && (DateFin != ""))
                    {
                        Condition = Condition + " AND {VueSuivieClient.Echeance} IN CDate({?DateDebut}) TO CDate({?DateFin})";
                    }

                }

            }
        }

        //**********************************
        Boolean Imprimerrr = false;
        public ReportDocument Rep = new ReportDocument();
        public void Frm_Impression_Load()
        {

            Rep.FileName = NomFichier;

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];

            foreach (CrystalDecisions.Shared.IConnectionInfo connection in Rep.DataSourceConnections)
            {

                for (int i = 0; i < Rep.DataSourceConnections.Count; i++)
                {
                    Rep.DataSourceConnections[i].SetConnection(Serveur, Db, Usr, Pwd);
                }

            }
            if (Condition != "")
            {
                Rep.RecordSelectionFormula = Condition;
            }

            foreach (CrystalDecisions.Shared.ParameterField Pr in Rep.ParameterFields)
            {
                if (Pr.Name == "MontantLettre")
                {
                    Rep.SetParameterValue(Pr.Name, MontantLettre);
                }

                if (Pr.Name == "@NumeroSession")
                {
                    Rep.SetParameterValue(Pr.Name, Session);
                }
                if (Pr.Name == "@DateJournee")
                {
                    Rep.SetParameterValue(Pr.Name, NumeroTicket);
                }
                if (Pr.Name == "@CodeDepot")
                {
                    Rep.SetParameterValue(Pr.Name, CodeDepot);
                }
                if (Pr.Name == "Depot")
                {

                    Rep.SetParameterValue(Pr.Name, Depot);
                }
                if (Pr.Name == "@CodeArticle")
                {
                    Rep.SetParameterValue(Pr.Name, CodeArticle);
                }
                if (Pr.Name == "@Nb_Etiquette")
                {
                    Rep.SetParameterValue(Pr.Name, NBFois);
                }
                if (Pr.Name == "Article")
                {
                    Rep.SetParameterValue(Pr.Name, Article);
                }
                if (Pr.Name == "Arrendissement")
                {
                    Rep.SetParameterValue(Pr.Name, Arrendissement);
                }
                if (Pr.Name == "Etat")
                {
                    Rep.SetParameterValue(Pr.Name, Etat);
                }
                if (Pr.Name == "@CodeClient")
                {
                    Rep.SetParameterValue(Pr.Name, CodeClient);
                }
                if (Pr.Name == "@CodeFournisseur")
                {
                    Rep.SetParameterValue(Pr.Name, CodeFournisseur);
                }
                if (Pr.Name == "CodeFournisseur")
                {
                    Rep.SetParameterValue(Pr.Name, CodeFournisseur);
                }

                if (Pr.Name == "@CodePersonnel")
                {
                    if (CodeFournisseur != null)
                        Rep.SetParameterValue(Pr.Name, CodeFournisseur);
                    else
                        Rep.SetParameterValue(Pr.Name, CodePersonnel);

                }
                if (Pr.Name == "RaisonSociale")
                {
                    // Client Cl_Client = new Client();
                    //Cl_Client.CodeClient = CodeClient;
                    // Cl_Client.obtenirInstanceClient(Cl_Client);

                    //  Rep.SetParameterValue(Pr.Name, Cl_Client.RaisonSociale);
                }


                if (Pr.Name == "RaisonSocialeFournisseur")
                {
                    //Fournisseur Cl_Fournisseur = new Fournisseur();
                    //Cl_Fournisseur.CodeFournisseur = CodeFournisseur;
                    //Cl_Fournisseur.obtenirInstanceFournisseur(Cl_Fournisseur);

                    //Rep.SetParameterValue(Pr.Name, Cl_Fournisseur.RaisonSociale);
                }


                if (Pr.Name == "@DateDebut")
                {
                    Rep.SetParameterValue(Pr.Name, DateDebut);
                }
                if (Pr.Name == "DateDebut")
                {
                    Rep.SetParameterValue(Pr.Name, DateDebut);
                }
                if (Pr.Name == "@CodeCompte")
                {
                    Rep.SetParameterValue(Pr.Name, CodeCompte);
                }
                if (Pr.Name == "NomCompte")
                {
                    Rep.SetParameterValue(Pr.Name, NomCompte);
                }
                if (Pr.Name == "DateFin")
                {
                    Rep.SetParameterValue(Pr.Name, DateFin);
                }
                if (Pr.Name == "@DateFin")
                {
                    Rep.SetParameterValue(Pr.Name, DateFin);
                }
                if (Pr.Name == "ListeArticle")
                {
                    Rep.SetParameterValue(Pr.Name, ListeArticle);
                }
                if (Pr.Name == "@Numcaisse")
                {
                    Rep.SetParameterValue(Pr.Name, Numcaisse);
                }
                if (Pr.Name == "@Nomutilisateur")
                {
                    Rep.SetParameterValue(Pr.Name, Nomutilisateur);
                }
                if (Pr.Name == "@DateJour")
                {
                    Rep.SetParameterValue(Pr.Name, DateDebut);
                }
                if (Pr.Name == "@NumeroImprimante")
                {
                    Rep.SetParameterValue(Pr.Name, NumeroImprimante);
                }
                if (Pr.Name == "@NumeroTicket")
                {
                    Rep.SetParameterValue(Pr.Name, NumeroTicket);
                }
                if (Pr.Name == "@NumeroSession")
                {
                    Rep.SetParameterValue(Pr.Name, Session);
                }
                if (Pr.Name == "@NomUtilisateur")
                {
                    Rep.SetParameterValue(Pr.Name, Caissier);
                }
            }
            if (Titre == "Liste Articles Vendu" || Titre == "TICKET" || Titre == "FactureVente" || Titre == "TICKET DE RETOUR" || Titre == "JOURNAL DE CAISSE" || Titre == "Stock Actuel")
            {
                try
                {
                    string Text = "Etat-[" + Titre + "]";
                    if (Titre == "JOURNAL DE CAISSE")
                    {
                        try
                        {
                            // Rep.SetParameterValue("CaissePV", Evenement.Caisse);
                            // Rep.SetParameterValue("CaissierPV", Evenement.NomUtilisateur);
                            Rep.SetParameterValue("DateJour", DateTime.Today);
                        }
                        catch
                        {
                        }
                    }

                    Imprimante Cl_Imprimante = new Imprimante();
                    String NomImprim = "";
                    if (Titre != "JOURNAL DE CAISSE")
                    {
                        Cl_Imprimante.NumeroImprimante = Convert.ToString(NumeroImprimante);
                        NomImprim = Cl_Imprimante.GetNomImprimante(Cl_Imprimante.NumeroImprimante);
                    }
                    else
                    {
                        Cl_Imprimante.NumeroImprimante = "2"; //annuler provisoirement pour imprimer sur imprimante Pizza pr tester le panne d'impression
                        NomImprim = Cl_Imprimante.GetNomImprimante(Cl_Imprimante.NumeroImprimante);
                    }

                    if (Titre == "FactureVente")
                    {
                        try
                        {
                            Cl_Imprimante = new Imprimante();
                            Cl_Imprimante.NumeroImprimante = "5";
                            NomImprim = Cl_Imprimante.GetNomImprimante(Cl_Imprimante.NumeroImprimante);
                        }
                        catch
                        {
                        }
                    }



                    Rep.PrintOptions.PrinterName = NomImprim;

                    if (Titre != "JOURNAL DE CAISSE")
                    {
                        if (Titre == "Liste Articles Vendu")
                        {
                            string Text1 = "Etat-[" + Titre + "]";
                            Rep.PrintToPrinter(1, false, 0, 0);
                        }
                        else
                        {
                            Rep.PrintToPrinter(1, false, 0, 0);
                        }
                    }
                    else
                    {
                        Rep.PrintToPrinter(1, false, 0, 0);
                    }
                }
                catch
                {
                }
            }
            if (Titre == "Consomation")
            {
                try
                {
                    string Text = "Etat-[" + Titre + "]";
                    //Rep.PrintOptions.PrinterName = Utilities.getInstance().lireNomImprimente("ImpTicket");
                    Rep.PrintToPrinter(1, false, 0, 0);
                    string Text1 = "Etat-[" + Titre + "]";
                }
                catch
                {
                }
            }
            if (Titre == "Demande de Prélévement")
            {
                try
                {
                    string Text = "Etat-[" + Titre + "]";
                    // Rep.PrintOptions.PrinterName = Utilities.getInstance().lireNomImprimente("ImpDemandePrelevement");
                    Rep.PrintToPrinter(1, false, 0, 0);
                    string Text1 = "Etat-[" + Titre + "]";
                }
                catch
                {
                }
            }

            if (Titre == "Ticket Restaurant" || Titre == "Ticket Pizza")
            {
                try
                {
                    string Text = "Etat-[" + Titre + "]";

                    Imprimante Cl_Imprimante = new Imprimante();
                    if (ParametreXml == true)
                    {
                        Rep.PrintOptions.PrinterName = ConfigurationImprimente;
                    }
                    else
                    {
                        Cl_Imprimante.NumeroImprimante = Convert.ToString(NumeroImprimante);
                        String NomImprim = Cl_Imprimante.GetNomImprimante(Cl_Imprimante.NumeroImprimante);
                        Rep.PrintOptions.PrinterName = NomImprim;
                    }

                    Rep.PrintToPrinter(1, false, 0, 0);
                }
                catch (Exception myErr)
                {

                }
            }
            else
            {
                try
                {
                    string Text = "Etat-[" + Titre + "]";
                }
                catch
                {
                }
            }
            if (PrintDirect)
            {
                Rep.PrintToPrinter(1, false, 0, 0);
                PrintDirect = false;
            }

        }

        Boolean PrintDirect = false;
    }
}