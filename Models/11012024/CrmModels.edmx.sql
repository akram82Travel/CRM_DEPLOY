
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/10/2022 09:16:28
-- Generated from EDMX file: D:\Projets 2022\Web_CRM\CrmModels.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [StubBorn];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Crm_ModuleNature_Crm_TacheNature]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Crm_ModuleNature] DROP CONSTRAINT [FK_Crm_ModuleNature_Crm_TacheNature];
GO
IF OBJECT_ID(N'[dbo].[FK_Crm_PlanningTime_Crm_Planning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Crm_PlanningTime] DROP CONSTRAINT [FK_Crm_PlanningTime_Crm_Planning];
GO
IF OBJECT_ID(N'[dbo].[FK_Utilisateur_Fonction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Utilisateur] DROP CONSTRAINT [FK_Utilisateur_Fonction];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[ClientCRM]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientCRM];
GO
IF OBJECT_ID(N'[dbo].[CompteurPiece]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompteurPiece];
GO
IF OBJECT_ID(N'[dbo].[ContactClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactClient];
GO
IF OBJECT_ID(N'[dbo].[ContratClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContratClient];
GO
IF OBJECT_ID(N'[dbo].[Crm_AccesUtilisateur]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_AccesUtilisateur];
GO
IF OBJECT_ID(N'[dbo].[Crm_ClientParent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_ClientParent];
GO
IF OBJECT_ID(N'[dbo].[Crm_ClientParentAssociation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_ClientParentAssociation];
GO
IF OBJECT_ID(N'[dbo].[Crm_ContactClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_ContactClient];
GO
IF OBJECT_ID(N'[dbo].[Crm_CycleExecTache]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_CycleExecTache];
GO
IF OBJECT_ID(N'[dbo].[Crm_Degres_Sanction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Degres_Sanction];
GO
IF OBJECT_ID(N'[dbo].[Crm_Equipe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Equipe];
GO
IF OBJECT_ID(N'[dbo].[Crm_HistoriqueType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_HistoriqueType];
GO
IF OBJECT_ID(N'[dbo].[Crm_Intervention]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Intervention];
GO
IF OBJECT_ID(N'[dbo].[crm_ModeTache]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crm_ModeTache];
GO
IF OBJECT_ID(N'[dbo].[Crm_ModuleNature]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_ModuleNature];
GO
IF OBJECT_ID(N'[dbo].[Crm_MoyenCommunication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_MoyenCommunication];
GO
IF OBJECT_ID(N'[dbo].[Crm_Notification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Notification];
GO
IF OBJECT_ID(N'[dbo].[Crm_OperationParTicket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_OperationParTicket];
GO
IF OBJECT_ID(N'[dbo].[Crm_Personne]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Personne];
GO
IF OBJECT_ID(N'[dbo].[Crm_PersonneConcerner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_PersonneConcerner];
GO
IF OBJECT_ID(N'[dbo].[Crm_PersonnelParRapoort]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_PersonnelParRapoort];
GO
IF OBJECT_ID(N'[dbo].[Crm_PersonneParReunion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_PersonneParReunion];
GO
IF OBJECT_ID(N'[dbo].[Crm_Planning]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Planning];
GO
IF OBJECT_ID(N'[dbo].[Crm_Planning_Responsable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Planning_Responsable];
GO
IF OBJECT_ID(N'[dbo].[Crm_PlanningTime]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_PlanningTime];
GO
IF OBJECT_ID(N'[dbo].[Crm_PVReunion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_PVReunion];
GO
IF OBJECT_ID(N'[dbo].[Crm_Rapport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Rapport];
GO
IF OBJECT_ID(N'[dbo].[Crm_ReclamationClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_ReclamationClient];
GO
IF OBJECT_ID(N'[dbo].[Crm_RespensableParIntervention]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_RespensableParIntervention];
GO
IF OBJECT_ID(N'[dbo].[Crm_Sanction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_Sanction];
GO
IF OBJECT_ID(N'[dbo].[Crm_TableObjet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TableObjet];
GO
IF OBJECT_ID(N'[dbo].[Crm_TacheCommercialSoft]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TacheCommercialSoft];
GO
IF OBJECT_ID(N'[dbo].[Crm_TacheExcution]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TacheExcution];
GO
IF OBJECT_ID(N'[dbo].[Crm_TacheLiee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TacheLiee];
GO
IF OBJECT_ID(N'[dbo].[Crm_TacheNature]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TacheNature];
GO
IF OBJECT_ID(N'[dbo].[Crm_TacheReclamation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TacheReclamation];
GO
IF OBJECT_ID(N'[dbo].[Crm_TicketClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TicketClient];
GO
IF OBJECT_ID(N'[dbo].[Crm_TypeOperation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TypeOperation];
GO
IF OBJECT_ID(N'[dbo].[Crm_TypeReclamation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TypeReclamation];
GO
IF OBJECT_ID(N'[dbo].[Crm_TypeTache]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Crm_TypeTache];
GO
IF OBJECT_ID(N'[dbo].[EmploiDuTemp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmploiDuTemp];
GO
IF OBJECT_ID(N'[dbo].[EtatCRM]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EtatCRM];
GO
IF OBJECT_ID(N'[dbo].[Fonction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Fonction];
GO
IF OBJECT_ID(N'[dbo].[PersonnelGRH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonnelGRH];
GO
IF OBJECT_ID(N'[dbo].[Respensable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Respensable];
GO
IF OBJECT_ID(N'[dbo].[Service]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Service];
GO
IF OBJECT_ID(N'[dbo].[Utilisateur]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utilisateur];
GO
IF OBJECT_ID(N'[CrmModelsStoreContainer].[Crm_Motif]', 'U') IS NOT NULL
    DROP TABLE [CrmModelsStoreContainer].[Crm_Motif];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ContactClient'
CREATE TABLE [dbo].[ContactClient] (
    [CodeClient] nvarchar(10)  NOT NULL,
    [id] int  NOT NULL,
    [Contact] nvarchar(50)  NOT NULL,
    [Tel] nvarchar(50)  NOT NULL,
    [Fax] nvarchar(50)  NOT NULL,
    [Mail] nvarchar(100)  NOT NULL,
    [SiteWeb] nvarchar(100)  NOT NULL,
    [CodeProfession] nvarchar(4)  NOT NULL
);
GO

-- Creating table 'Crm_AccesUtilisateur'
CREATE TABLE [dbo].[Crm_AccesUtilisateur] (
    [NomUtilisateur] nvarchar(20)  NOT NULL,
    [CodeAcces] nvarchar(5)  NOT NULL
);
GO

-- Creating table 'Crm_ContactClient'
CREATE TABLE [dbo].[Crm_ContactClient] (
    [CodeCLient] nvarchar(50)  NOT NULL,
    [IdContact] int IDENTITY(1,1) NOT NULL,
    [NomContact] nvarchar(150)  NOT NULL,
    [TelContact] nvarchar(50)  NOT NULL,
    [MailContact] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_CycleExecTache'
CREATE TABLE [dbo].[Crm_CycleExecTache] (
    [NumeroTache] nvarchar(50)  NOT NULL,
    [Nomutilisateur] nvarchar(50)  NOT NULL,
    [OldStatus] nvarchar(4)  NOT NULL,
    [NewStatus] nvarchar(4)  NOT NULL,
    [DateDebutExecution] datetime  NOT NULL,
    [DateFinExecution] datetime  NOT NULL,
    [Duree] int  NOT NULL,
    [idTacheExec] int IDENTITY(1,1) NOT NULL,
    [Encours] bit  NOT NULL
);
GO

-- Creating table 'Crm_Intervention'
CREATE TABLE [dbo].[Crm_Intervention] (
    [NumeroIntervention] nvarchar(50)  NOT NULL,
    [DateInterventionDebut] datetime  NOT NULL,
    [DateInterventionFin] datetime  NOT NULL,
    [DateIntervention] datetime  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [Createur] nvarchar(50)  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSociale] nvarchar(50)  NOT NULL,
    [Duree] nvarchar(50)  NOT NULL,
    [Visite] bit  NOT NULL,
    [Local] bit  NOT NULL,
    [CodeVoiture] nvarchar(50)  NOT NULL,
    [DescriptionIntervention] nvarchar(500)  NOT NULL,
    [Voiture] bit  NOT NULL,
    [NumeroDossier] nvarchar(50)  NOT NULL,
    [Paye] bit  NOT NULL,
    [Nature] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_MoyenCommunication'
CREATE TABLE [dbo].[Crm_MoyenCommunication] (
    [CodeMoyenCommunication] nvarchar(50)  NOT NULL,
    [Libelle] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_Notification'
CREATE TABLE [dbo].[Crm_Notification] (
    [NumeroNotification] int IDENTITY(1,1) NOT NULL,
    [NomTable] nvarchar(50)  NOT NULL,
    [NumeroPiece] nvarchar(50)  NOT NULL,
    [Notifier] bit  NOT NULL,
    [Piece] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_OperationParTicket'
CREATE TABLE [dbo].[Crm_OperationParTicket] (
    [IdOperation] nvarchar(50)  NOT NULL,
    [IdTicket] nvarchar(50)  NOT NULL,
    [CodeTypeOperation] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_PersonnelParRapoort'
CREATE TABLE [dbo].[Crm_PersonnelParRapoort] (
    [NumeroRapport] nvarchar(50)  NOT NULL,
    [IdPresen] int IDENTITY(1,1) NOT NULL,
    [RaisonSociale] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_RespensableParIntervention'
CREATE TABLE [dbo].[Crm_RespensableParIntervention] (
    [CodeRespensable] nvarchar(50)  NOT NULL,
    [NumeroIntervention] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_TableObjet'
CREATE TABLE [dbo].[Crm_TableObjet] (
    [NumObjet] nvarchar(50)  NOT NULL,
    [Descripotion] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Crm_TacheExcution'
CREATE TABLE [dbo].[Crm_TacheExcution] (
    [IdTache] nvarchar(50)  NOT NULL,
    [Nomutilisateur] nvarchar(50)  NOT NULL,
    [OldEtat] nvarchar(4)  NOT NULL,
    [OldStatus] nvarchar(4)  NOT NULL,
    [NewEtat] nvarchar(4)  NOT NULL,
    [NewStatus] nvarchar(4)  NOT NULL,
    [DateExcution] datetime  NOT NULL,
    [DateFinExcution] datetime  NOT NULL,
    [Duree] int  NOT NULL,
    [IdtacheExce] int IDENTITY(1,1) NOT NULL,
    [DescriptionAcorriger] nvarchar(2500)  NOT NULL,
    [DescriptionAValider] nvarchar(2500)  NOT NULL,
    [DescriptionAExpliquer] nvarchar(2500)  NOT NULL
);
GO

-- Creating table 'Crm_TacheLiee'
CREATE TABLE [dbo].[Crm_TacheLiee] (
    [NumeroTache] nvarchar(50)  NOT NULL,
    [CodeRespensable] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_TacheReclamation'
CREATE TABLE [dbo].[Crm_TacheReclamation] (
    [NumeroTache] nvarchar(50)  NOT NULL,
    [NumeroOrdre] int  NOT NULL,
    [NumDossier] nvarchar(50)  NOT NULL,
    [TacheTitre] nvarchar(100)  NOT NULL,
    [DescriptionTache] nvarchar(2500)  NOT NULL,
    [Nature] nvarchar(50)  NOT NULL,
    [Type] nvarchar(50)  NOT NULL,
    [Degres] int  NOT NULL,
    [NomPlanificateur] nvarchar(50)  NOT NULL,
    [NomValidateur] nvarchar(50)  NOT NULL,
    [NomTesteur] nvarchar(50)  NOT NULL,
    [CodeRespensable] nvarchar(50)  NOT NULL,
    [Duree] nvarchar(50)  NOT NULL,
    [SalaireNet] decimal(18,3)  NOT NULL,
    [Coef] decimal(18,3)  NOT NULL,
    [MontantCharge] decimal(18,3)  NOT NULL,
    [NumeroEtat] nvarchar(50)  NOT NULL,
    [Status] nvarchar(50)  NOT NULL,
    [EtatValidation] nvarchar(10)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [UtilisateurCreateur] nvarchar(50)  NOT NULL,
    [Paye] bit  NOT NULL,
    [TypePiece] nvarchar(20)  NOT NULL,
    [NumPiece] nvarchar(20)  NOT NULL,
    [ValiderPar] nvarchar(50)  NOT NULL,
    [AExpliquer] bit  NOT NULL,
    [IndicateurACorriger] int  NOT NULL,
    [IndicateurAValider] int  NOT NULL,
    [IndicateurAExpliquer] int  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSociale] nvarchar(200)  NOT NULL,
    [AnuulerPar] nvarchar(50)  NOT NULL,
    [DateAnnulation] nvarchar(50)  NOT NULL,
    [ConfirmerPar] nvarchar(50)  NOT NULL,
    [DateConfirmation] nvarchar(50)  NOT NULL,
    [NonConfirmerPar] nvarchar(50)  NOT NULL,
    [DateNonConfirmation] nvarchar(50)  NOT NULL,
    [DatePrevus] datetime  NOT NULL,
    [DateDebutExecution] nvarchar(50)  NOT NULL,
    [DateFinExecution] nvarchar(50)  NOT NULL,
    [DurerExecution] int  NOT NULL,
    [DurerExcutionValider] nvarchar(50)  NOT NULL,
    [DureeReel] int  NOT NULL,
    [NumReclamation] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_TicketClient'
CREATE TABLE [dbo].[Crm_TicketClient] (
    [IdTicket] nvarchar(50)  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSociale] nvarchar(50)  NOT NULL,
    [Titre] nvarchar(50)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [Createur] nvarchar(50)  NOT NULL,
    [DatePrisEnCharge] nvarchar(50)  NOT NULL,
    [PrisEnChargePar] nvarchar(50)  NOT NULL,
    [DateTicket] datetime  NOT NULL
);
GO

-- Creating table 'Crm_TypeOperation'
CREATE TABLE [dbo].[Crm_TypeOperation] (
    [CodeTypeOperation] nvarchar(50)  NOT NULL,
    [Libelle] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_TypeReclamation'
CREATE TABLE [dbo].[Crm_TypeReclamation] (
    [CodeTypeReclamation] nvarchar(10)  NOT NULL,
    [Libelle] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'EtatCRM'
CREATE TABLE [dbo].[EtatCRM] (
    [NumeroEtat] nvarchar(4)  NOT NULL,
    [Libelle] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'Respensable'
CREATE TABLE [dbo].[Respensable] (
    [CodeRespensable] nvarchar(3)  NOT NULL,
    [Nom] nvarchar(50)  NOT NULL,
    [TC1] decimal(18,3)  NOT NULL,
    [TC2] decimal(18,3)  NOT NULL,
    [TC3] decimal(18,3)  NOT NULL,
    [TC4] decimal(18,3)  NOT NULL,
    [TC5] decimal(18,3)  NOT NULL,
    [TC6] decimal(18,3)  NOT NULL,
    [TC7] decimal(18,3)  NOT NULL,
    [TP1] decimal(18,3)  NOT NULL,
    [TP2] decimal(18,3)  NOT NULL,
    [TC8] decimal(18,3)  NOT NULL,
    [TP3] decimal(18,3)  NOT NULL,
    [TP4] decimal(18,3)  NOT NULL,
    [Commercial] bit  NOT NULL,
    [MatriculePersonnel] nvarchar(30)  NOT NULL,
    [CodeService] nvarchar(50)  NOT NULL,
    [CodeSuperieurHiearchique] nvarchar(50)  NULL,
    [Actif] bit  NOT NULL,
    [SupHier] bit  NOT NULL
);
GO

-- Creating table 'Utilisateur'
CREATE TABLE [dbo].[Utilisateur] (
    [NomUtilisateur] nvarchar(20)  NOT NULL,
    [CodeSociete] nvarchar(6)  NOT NULL,
    [CodeFonction] nvarchar(6)  NOT NULL,
    [Nom] nvarchar(30)  NOT NULL,
    [Prenom] nvarchar(30)  NOT NULL,
    [MotDePasse] nvarchar(20)  NOT NULL,
    [Skin] nvarchar(50)  NOT NULL,
    [CodeRepresentant] nvarchar(10)  NOT NULL,
    [MotDePasseValidation] nvarchar(20)  NOT NULL,
    [MotDePasseAnnulation] nvarchar(20)  NOT NULL,
    [MDPValidation] nvarchar(20)  NOT NULL,
    [MDPReimpression] nvarchar(20)  NOT NULL,
    [SaisieLibreLivraisonVente] bit  NOT NULL,
    [SaisieLibreReceptionAchat] bit  NOT NULL,
    [CodeEmployer] nvarchar(20)  NOT NULL,
    [Admin] bit  NOT NULL,
    [Administrateur] bit  NOT NULL,
    [CreerPar] nvarchar(50)  NOT NULL,
    [CreerLe] nvarchar(50)  NOT NULL,
    [Actif] bit  NOT NULL,
    [SaisieLibreRetourVente] bit  NOT NULL,
    [SaisieLibreRetourAchat] bit  NOT NULL,
    [CodeService] nvarchar(20)  NOT NULL,
    [MDPAdministrateur] nvarchar(20)  NOT NULL,
    [CodeIntervenant] nvarchar(10)  NOT NULL,
    [CodeFonctionAndroid] nvarchar(6)  NOT NULL,
    [SaisieLibreNotification] bit  NOT NULL
);
GO

-- Creating table 'Crm_Motif'
CREATE TABLE [dbo].[Crm_Motif] (
    [CodeMotif] nvarchar(10)  NOT NULL,
    [Libelle] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Fonction'
CREATE TABLE [dbo].[Fonction] (
    [CodeFonction] nvarchar(6)  NOT NULL,
    [Libelle] nvarchar(60)  NOT NULL,
    [CodeSuperieurHiearchique] nvarchar(10)  NOT NULL,
    [HorraireMaxSortie] decimal(18,2)  NULL
);
GO

-- Creating table 'Service'
CREATE TABLE [dbo].[Service] (
    [CodeService] nvarchar(50)  NOT NULL,
    [Libelle] nvarchar(50)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [NomUtilisateur] nvarchar(50)  NOT NULL,
    [CodeSuperieur] nvarchar(50)  NULL
);
GO

-- Creating table 'Crm_Personne'
CREATE TABLE [dbo].[Crm_Personne] (
    [CodePersonne] nvarchar(50)  NOT NULL,
    [NomPrenom] nvarchar(80)  NOT NULL,
    [Adresse] nvarchar(80)  NOT NULL,
    [Nationalite] nvarchar(30)  NOT NULL,
    [Tel] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CompteurPiece'
CREATE TABLE [dbo].[CompteurPiece] (
    [CodeSociete] nvarchar(6)  NOT NULL,
    [NomPiecer] nvarchar(35)  NOT NULL,
    [PrefixPiece] nvarchar(4)  NOT NULL,
    [Annee] nvarchar(3)  NOT NULL,
    [Compteur] nvarchar(7)  NOT NULL
);
GO

-- Creating table 'Crm_ReclamationClient'
CREATE TABLE [dbo].[Crm_ReclamationClient] (
    [Id_Reclamation] nvarchar(50)  NOT NULL,
    [Titre] nvarchar(150)  NOT NULL,
    [Description] nvarchar(2500)  NOT NULL,
    [DateReclamation] datetime  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSociale] nvarchar(250)  NOT NULL,
    [Soft] bit  NOT NULL,
    [Technique] bit  NOT NULL,
    [NomContact] nvarchar(50)  NOT NULL,
    [CodeMoyenCommunication] nvarchar(50)  NOT NULL,
    [OutilCommunication] nvarchar(100)  NOT NULL,
    [Observation] nvarchar(200)  NOT NULL,
    [Createur] nvarchar(50)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [logo] nvarchar(50)  NOT NULL,
    [Status] nvarchar(5)  NOT NULL,
    [NumeroDossier] nvarchar(50)  NOT NULL,
    [PrisEnChargePar] nvarchar(50)  NOT NULL,
    [DatePrisEnCharge] nvarchar(50)  NOT NULL,
    [ObjetReclamation] nvarchar(500)  NOT NULL,
    [Paye] bit  NOT NULL,
    [TypeReclamation] nvarchar(50)  NOT NULL,
    [Commercial] bit  NOT NULL,
    [UtilisateurContact] nvarchar(50)  NOT NULL,
    [Duree] int  NOT NULL,
    [Nature] nvarchar(50)  NULL,
    [IdNatureType] int  NULL
);
GO

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [CodeClient] nvarchar(10)  NOT NULL,
    [Respensable] nvarchar(300)  NOT NULL,
    [CodeForme] nvarchar(6)  NOT NULL,
    [CodeFamilleClient] nvarchar(6)  NOT NULL,
    [CodeRepresentant] nvarchar(3)  NOT NULL,
    [Tel1] nvarchar(50)  NOT NULL,
    [Fax1] nvarchar(50)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [SiteWeb] nvarchar(100)  NOT NULL,
    [RaisonSociale] nvarchar(500)  NOT NULL,
    [RegistreCommerce] nvarchar(20)  NOT NULL,
    [SeuilCredit] decimal(18,3)  NOT NULL,
    [SoldeCredit] decimal(18,3)  NOT NULL,
    [SeuilEngagement] decimal(18,3)  NOT NULL,
    [SoldeEngagement] decimal(18,3)  NOT NULL,
    [BlocageFacturation] bit  NOT NULL,
    [Adresse1] nvarchar(1000)  NOT NULL,
    [CodeSousRegion] nvarchar(4)  NOT NULL,
    [CodeZone] nvarchar(4)  NOT NULL,
    [CodePostale] nvarchar(10)  NOT NULL,
    [CodeVille] nvarchar(30)  NOT NULL,
    [AresseLivraison] nvarchar(160)  NOT NULL,
    [CodePostaleLivraison] nvarchar(10)  NOT NULL,
    [Exoneration] bit  NOT NULL,
    [NumeroExonoration] nvarchar(20)  NOT NULL,
    [DateExonoration] datetime  NOT NULL,
    [DateFinExonoration] datetime  NOT NULL,
    [PayerTVA] bit  NOT NULL,
    [Export] bit  NOT NULL,
    [NumeroExport] nvarchar(30)  NOT NULL,
    [CodeRemise] nvarchar(2)  NOT NULL,
    [MartriculeFiscale] nvarchar(20)  NOT NULL,
    [TimbreFiscal] bit  NOT NULL,
    [CodeBanque] nvarchar(6)  NOT NULL,
    [RibBanquaire] nvarchar(25)  NOT NULL,
    [CodeModeReglement] nvarchar(6)  NOT NULL,
    [TauxRemise] decimal(18,3)  NOT NULL,
    [Observation] nvarchar(max)  NOT NULL,
    [Assujetti] bit  NOT NULL,
    [NbJourReglement] decimal(18,3)  NOT NULL,
    [SignalReglement] decimal(18,3)  NOT NULL,
    [Fodec] bit  NOT NULL,
    [CIN] nvarchar(20)  NOT NULL,
    [CodeComptable] nvarchar(10)  NOT NULL,
    [Comptabiliser] bit  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [UtilisateurCreation] nvarchar(20)  NOT NULL,
    [CalculAutoSeuilCredit] bit  NOT NULL,
    [RemiseDepassement] bit  NOT NULL,
    [Taxe] bit  NOT NULL,
    [DepassementSeuil] decimal(18,3)  NOT NULL,
    [DivCredit] decimal(18,3)  NOT NULL,
    [Inactif] bit  NOT NULL,
    [Contentieux] bit  NOT NULL,
    [ObservationGarantie] nvarchar(500)  NOT NULL,
    [ContactAdresseLivraison] nvarchar(50)  NOT NULL,
    [CodeTypeDecharge] nvarchar(4)  NOT NULL,
    [DateOuvertureSociete] datetime  NOT NULL,
    [NomGerant] nvarchar(100)  NOT NULL,
    [DateNaissanceGerant] datetime  NOT NULL,
    [Avis] nvarchar(max)  NOT NULL,
    [CINLivraison] nvarchar(50)  NOT NULL,
    [MatriculeFiscaleLivraison] nvarchar(50)  NOT NULL,
    [DateOuvertureLivraison] datetime  NOT NULL,
    [ManqueDocument] bit  NOT NULL,
    [Prospect] bit  NOT NULL,
    [DateProspect] datetime  NOT NULL,
    [CodeRespensableAdministration] nvarchar(4)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Promo] bit  NOT NULL,
    [Comptoir] bit  NOT NULL,
    [Classic] bit  NOT NULL,
    [ChangerRaisonSociale] bit  NOT NULL,
    [CodeDevise] nvarchar(4)  NOT NULL,
    [Extention] nvarchar(10)  NOT NULL,
    [Login] nvarchar(30)  NOT NULL,
    [Logo] varbinary(max)  NULL,
    [ClientFournisseur] bit  NOT NULL,
    [CodeTypeParrain] nvarchar(4)  NOT NULL,
    [CodeParrain] nvarchar(8)  NOT NULL,
    [CodePays] nvarchar(4)  NOT NULL,
    [CodeLangue] nvarchar(4)  NOT NULL,
    [CodeSousFamille] nvarchar(4)  NOT NULL,
    [DroitConsommation] bit  NOT NULL,
    [AfficherNbPallete] bit  NOT NULL,
    [BlocagePreavis] bit  NOT NULL,
    [NbPreavis] int  NOT NULL,
    [RemiseQuantite] bit  NOT NULL,
    [RegimeReel] bit  NOT NULL,
    [Capital] decimal(18,3)  NOT NULL,
    [AutoriserSeuilSuplementaire] bit  NOT NULL,
    [DateAutorisationSuplementaire] nvarchar(10)  NOT NULL,
    [NbJourSuplementaire] int  NOT NULL,
    [ValeurCreditSuplementaire] decimal(18,0)  NOT NULL,
    [ValeurEngagementSuplementaire] decimal(18,0)  NOT NULL,
    [PrixUsine] bit  NOT NULL,
    [CodeVilleLivraison] nvarchar(60)  NOT NULL,
    [DateTelevente] nvarchar(10)  NOT NULL,
    [DateLivraison] nvarchar(10)  NOT NULL,
    [DateRepresentant] nvarchar(10)  NOT NULL,
    [DateOuverturePatente] nvarchar(30)  NOT NULL,
    [Effectif] int  NOT NULL,
    [CodeFournisseur] nvarchar(10)  NOT NULL,
    [AutreAdresse] bit  NOT NULL,
    [NomValidateur] nvarchar(20)  NOT NULL,
    [DateValidation] nvarchar(20)  NOT NULL,
    [SeuilDepassementCredit] decimal(18,3)  NOT NULL,
    [RecalculSoldeMois] decimal(18,3)  NOT NULL,
    [DepassementBL] decimal(18,3)  NOT NULL,
    [Agreer] bit  NOT NULL,
    [AnnulerTousControle] bit  NOT NULL,
    [AutoriserFactoring] bit  NOT NULL,
    [MontantAutorisation] decimal(18,3)  NOT NULL,
    [LogoCachet] varbinary(max)  NULL,
    [TotaleGarantie] decimal(18,3)  NOT NULL,
    [AvecGaranti] bit  NOT NULL,
    [LogoCachet2] varbinary(max)  NULL,
    [DateDerniereModification] datetime  NOT NULL,
    [DerniereModification] nvarchar(20)  NOT NULL,
    [NumeroCarte] nvarchar(50)  NOT NULL,
    [SoldePoint] decimal(18,3)  NOT NULL,
    [SoldeValeurPoint] decimal(18,3)  NOT NULL,
    [ActiverDepassementPieceNonPaye] bit  NOT NULL,
    [DateAutorisationPieceNonPaye] nvarchar(10)  NOT NULL,
    [NBAutorisationPieceNonPaye] int  NOT NULL,
    [FraisLogistique] decimal(18,2)  NOT NULL,
    [TypeClient] nvarchar(255)  NOT NULL,
    [CodeFournisseurAutreBase] nvarchar(10)  NOT NULL,
    [NomAutreBase] nvarchar(100)  NOT NULL,
    [VIP] bit  NOT NULL,
    [Terme] bit  NOT NULL,
    [DefalcationBL] bit  NOT NULL,
    [CodeClientParent] nvarchar(50)  NULL,
    [Promotion] bit  NOT NULL,
    [ToleranceVariationQuantiteFabrication] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Crm_PersonneParReunion'
CREATE TABLE [dbo].[Crm_PersonneParReunion] (
    [NumeroPVReunion] nvarchar(50)  NOT NULL,
    [CodePersonne] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_PVReunion'
CREATE TABLE [dbo].[Crm_PVReunion] (
    [NumeroPVReunion] nvarchar(50)  NOT NULL,
    [Projet] nvarchar(50)  NOT NULL,
    [Module] nvarchar(50)  NOT NULL,
    [Createur] nvarchar(50)  NOT NULL,
    [Objet] nvarchar(200)  NOT NULL,
    [DateReunion] datetime  NOT NULL,
    [DatePrevusReunion] datetime  NOT NULL,
    [CodeClient] nvarchar(10)  NOT NULL,
    [CodePersonne] nvarchar(50)  NOT NULL,
    [DescriptionGeneral] varchar(max)  NOT NULL,
    [Dure] time  NULL
);
GO

-- Creating table 'Crm_Rapport'
CREATE TABLE [dbo].[Crm_Rapport] (
    [NumeroRapport] nvarchar(50)  NOT NULL,
    [DescriptionRapport] nvarchar(2500)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [UtilisateurCreateur] nvarchar(50)  NOT NULL,
    [Degres] int  NOT NULL,
    [NumeroEtat] nvarchar(50)  NOT NULL,
    [NumeroOrdre] int  NOT NULL,
    [DatePrevusProchaineReunion] nvarchar(50)  NOT NULL,
    [Duree] int  NULL,
    [Status] nvarchar(50)  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSociale] nvarchar(500)  NOT NULL,
    [NumeroDossier] nvarchar(50)  NOT NULL,
    [DateRapport] datetime  NOT NULL
);
GO

-- Creating table 'Crm_PersonneConcerner'
CREATE TABLE [dbo].[Crm_PersonneConcerner] (
    [NumeroReclamation] nvarchar(50)  NOT NULL,
    [NomUtilisateur] nvarchar(50)  NOT NULL,
    [Vue] bit  NOT NULL,
    [DateVue] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_Planning'
CREATE TABLE [dbo].[Crm_Planning] (
    [Id_Planning] nvarchar(50)  NOT NULL,
    [Datedebut] datetime  NULL,
    [DateFin] datetime  NULL,
    [Titre] nvarchar(200)  NULL,
    [Description] varchar(max)  NOT NULL,
    [CodeResponsable] nvarchar(50)  NOT NULL,
    [Nom] nvarchar(100)  NOT NULL,
    [Prenom] nvarchar(100)  NOT NULL,
    [CodeClient] nvarchar(50)  NOT NULL,
    [RaisonSocial] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Crm_Planning_Responsable'
CREATE TABLE [dbo].[Crm_Planning_Responsable] (
    [CodeRespensable] nvarchar(50)  NOT NULL,
    [Id_Planning] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_PlanningTime'
CREATE TABLE [dbo].[Crm_PlanningTime] (
    [Id_Ligne] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Titre] nvarchar(200)  NULL,
    [Id_Plannig] nvarchar(50)  NOT NULL,
    [CodeResponsable] nvarchar(50)  NOT NULL,
    [Matin] nvarchar(200)  NULL,
    [ApresMidi] nvarchar(200)  NULL,
    [CodeClient] nvarchar(50)  NULL,
    [RaisonSociale] nvarchar(50)  NULL,
    [MatinReel] nvarchar(200)  NULL,
    [ApresMidiReel] nvarchar(200)  NULL,
    [ObjectisNonRealiser] varchar(max)  NULL
);
GO

-- Creating table 'ClientCRM'
CREATE TABLE [dbo].[ClientCRM] (
    [CodeClient] nvarchar(10)  NOT NULL,
    [RaisonSociale] nvarchar(500)  NOT NULL,
    [Adresse1] nvarchar(1000)  NOT NULL,
    [MartriculeFiscale] nvarchar(20)  NOT NULL,
    [Tel1] nvarchar(50)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [CodeClientCommercial] nvarchar(10)  NOT NULL,
    [MotPasse] nvarchar(50)  NULL,
    [imageElement] varbinary(max)  NULL,
    [FileName] varchar(max)  NULL,
    [imagephysique] varchar(50)  NULL
);
GO

-- Creating table 'Crm_TacheNature'
CREATE TABLE [dbo].[Crm_TacheNature] (
    [Nature] varchar(50)  NOT NULL,
    [Libelle] varchar(50)  NULL
);
GO

-- Creating table 'crm_ModeTache'
CREATE TABLE [dbo].[crm_ModeTache] (
    [CodeModeTache] nvarchar(10)  NOT NULL,
    [Libelle] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_ModuleNature'
CREATE TABLE [dbo].[Crm_ModuleNature] (
    [IdNatureType] int IDENTITY(1,1) NOT NULL,
    [Libelle] varchar(50)  NULL,
    [Nature] varchar(50)  NOT NULL
);
GO

-- Creating table 'ContratClient'
CREATE TABLE [dbo].[ContratClient] (
    [NumeroContratClient] nvarchar(10)  NOT NULL,
    [DateContratClient] datetime  NOT NULL,
    [CodeClient] nvarchar(10)  NOT NULL,
    [CodeTypeContrat] nvarchar(10)  NOT NULL,
    [CodeArticle] nvarchar(30)  NOT NULL,
    [Actif] bit  NOT NULL,
    [DateDebut] datetime  NOT NULL,
    [DateFin] datetime  NOT NULL,
    [OccuranceFacturation] int  NOT NULL,
    [MontantHT] decimal(18,3)  NOT NULL,
    [DatePremiereFactration] datetime  NOT NULL,
    [SuiteContrat] bit  NOT NULL,
    [TauxRemise] decimal(18,2)  NOT NULL,
    [ModeFacturation] nvarchar(20)  NOT NULL,
    [Observation] nvarchar(500)  NOT NULL,
    [NomUtilisateur] nvarchar(20)  NOT NULL,
    [DateCreation] datetime  NULL,
    [HeureCreation] datetime  NULL,
    [DateFinProlongation] datetime  NOT NULL
);
GO

-- Creating table 'Crm_TacheCommercialSoft'
CREATE TABLE [dbo].[Crm_TacheCommercialSoft] (
    [NumeroTacheCommercial] nvarchar(50)  NOT NULL,
    [NumeroTacheSoft_Tecnique] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'EmploiDuTemp'
CREATE TABLE [dbo].[EmploiDuTemp] (
    [NumeroEmploiTemp] nvarchar(10)  NOT NULL,
    [CodeGroupe] nvarchar(10)  NOT NULL,
    [Mois] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'PersonnelGRH'
CREATE TABLE [dbo].[PersonnelGRH] (
    [MatriculePersonnel] nvarchar(30)  NOT NULL,
    [NomPrenom] nvarchar(80)  NOT NULL,
    [Lieu] nvarchar(80)  NOT NULL,
    [CCB] nvarchar(30)  NOT NULL,
    [Adresse] nvarchar(80)  NOT NULL,
    [CIN] nvarchar(30)  NOT NULL,
    [Nationalite] nvarchar(30)  NOT NULL,
    [Sexe] nvarchar(1)  NOT NULL,
    [CodeEtatCivil] nvarchar(15)  NOT NULL,
    [NB_Enfant] int  NOT NULL,
    [DateEmbauge] nvarchar(30)  NOT NULL,
    [DateCNSS] nvarchar(30)  NOT NULL,
    [ChefFamille] bit  NOT NULL,
    [CodeOrganigramme] nvarchar(10)  NOT NULL,
    [CodeQualification] nvarchar(10)  NOT NULL,
    [CodeRegime] nvarchar(10)  NOT NULL,
    [CodeCategorie] nvarchar(10)  NOT NULL,
    [CodeEchelon] nvarchar(10)  NOT NULL,
    [CoefConge] decimal(18,2)  NOT NULL,
    [CodeNature] nvarchar(10)  NOT NULL,
    [SalaireBase] decimal(18,3)  NOT NULL,
    [CodeTypeContrat] nvarchar(10)  NOT NULL,
    [CodeTypeCNSS] nvarchar(10)  NOT NULL,
    [EnActivite] bit  NOT NULL,
    [SoumisImpot] bit  NOT NULL,
    [ParentEnCharge] bit  NOT NULL,
    [Observation] nvarchar(max)  NOT NULL,
    [CodeModePaiement] nvarchar(6)  NULL,
    [CodeBanque] nvarchar(6)  NULL,
    [MatriculeAssureSocial] nvarchar(8)  NOT NULL,
    [CleAssureSocial] nvarchar(2)  NOT NULL,
    [Nom] nvarchar(20)  NOT NULL,
    [Prenom] nvarchar(20)  NOT NULL,
    [PrenomPere] nvarchar(20)  NOT NULL,
    [NbParentACharge] int  NULL,
    [NumCNSS] nvarchar(15)  NOT NULL,
    [DateNaissance] nvarchar(30)  NULL,
    [DateCIN] nvarchar(30)  NULL,
    [DateDebutContrat] nvarchar(30)  NOT NULL,
    [DateFinContrat] nvarchar(30)  NOT NULL,
    [DateSortie] nvarchar(30)  NOT NULL,
    [TypeCNSS2] nvarchar(5)  NOT NULL,
    [DetailEnfants] bit  NOT NULL,
    [Tel] nvarchar(30)  NOT NULL,
    [NomConjoint] nvarchar(50)  NOT NULL,
    [ConjointTravail] bit  NOT NULL,
    [DateValiditeCHandicape] nvarchar(30)  NOT NULL,
    [CodeChargePatronale] nvarchar(10)  NULL,
    [NiveauPersonnel] nvarchar(100)  NOT NULL,
    [CodeAssurance] nvarchar(10)  NOT NULL,
    [CotationAssurance] decimal(18,2)  NOT NULL,
    [DateAssurance] nvarchar(30)  NOT NULL,
    [AssureChargeSociete] bit  NOT NULL,
    [MatriculePersonnelNew] nvarchar(30)  NOT NULL,
    [Photo] varbinary(max)  NULL,
    [Session] nvarchar(10)  NOT NULL,
    [CodeEmplacement] nvarchar(10)  NOT NULL,
    [CodeTypeProductivité] nvarchar(10)  NOT NULL,
    [CodeSociete] nvarchar(10)  NOT NULL,
    [NomPrenomArabe] nvarchar(30)  NOT NULL,
    [AdresseArabe] nvarchar(80)  NOT NULL,
    [QualificationArabe] nvarchar(30)  NOT NULL,
    [anciensalairebasegrille] decimal(18,3)  NOT NULL,
    [RevenuReinvesti] bit  NOT NULL,
    [DateReinvesti] nvarchar(20)  NOT NULL,
    [MontantReinvesti] decimal(18,3)  NOT NULL,
    [FraisPro] bit  NOT NULL,
    [NumeroPermis] nvarchar(30)  NOT NULL,
    [DateFin] nvarchar(30)  NOT NULL,
    [CodeCategoriePermis] nvarchar(10)  NOT NULL,
    [ClePermis] nvarchar(2)  NOT NULL,
    [SalaireNet] decimal(18,3)  NOT NULL,
    [Prospect] bit  NOT NULL,
    [TelAutre] nvarchar(20)  NOT NULL,
    [TelDomicile] nvarchar(20)  NOT NULL,
    [CodeService] nvarchar(20)  NOT NULL,
    [CodeSite] nvarchar(20)  NOT NULL,
    [NomUtilisateur] nvarchar(20)  NOT NULL,
    [DateCreation] datetime  NOT NULL,
    [HeureCreation] datetime  NOT NULL,
    [CodeSupHier] nvarchar(30)  NOT NULL,
    [AppliquerIndemnité] bit  NOT NULL,
    [ChiffreAffaireEstime] decimal(18,3)  NOT NULL,
    [MontantAdeduireCAInferieur] decimal(18,3)  NOT NULL,
    [MontantHeurSuppl] decimal(18,3)  NOT NULL,
    [existedansPointeuse] bit  NOT NULL,
    [NbFingerPrint] int  NOT NULL,
    [DateDebutEchelon] nvarchar(10)  NOT NULL,
    [DateEchangeEchelon] nvarchar(10)  NOT NULL,
    [DateDebutDomSalaire] nvarchar(60)  NOT NULL,
    [DateFinDomSalaire] nvarchar(60)  NOT NULL,
    [DemandeDomSalaire] bit  NOT NULL,
    [Email] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_ClientParent'
CREATE TABLE [dbo].[Crm_ClientParent] (
    [CodeClientParent] nvarchar(50)  NOT NULL,
    [Libelle] nvarchar(100)  NULL
);
GO

-- Creating table 'Crm_ClientParentAssociation'
CREATE TABLE [dbo].[Crm_ClientParentAssociation] (
    [CodeClientParent] nvarchar(50)  NOT NULL,
    [CodeClientFils] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Crm_Equipe'
CREATE TABLE [dbo].[Crm_Equipe] (
    [CodeEquipe] nvarchar(10)  NOT NULL,
    [Libelle] nvarchar(100)  NOT NULL,
    [ChefEquipe] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Crm_TypeTache'
CREATE TABLE [dbo].[Crm_TypeTache] (
    [CodeTypeTache] varchar(50)  NOT NULL,
    [Libelle] varchar(50)  NULL,
    [mode] varchar(50)  NULL
);
GO

-- Creating table 'Crm_Sanction'
CREATE TABLE [dbo].[Crm_Sanction] (
    [IdSanction] int IDENTITY(1,1) NOT NULL,
    [LibelleSanction] varchar(50)  NOT NULL,
    [NoteSanction] varchar(50)  NOT NULL,
    [TypeTache] varchar(50)  NOT NULL,
    [ModeTache] varchar(50)  NOT NULL,
    [CodeUtilisateur] varchar(50)  NOT NULL,
    [Degres] int  NULL,
    [TypeSanction] varchar(50)  NULL
);
GO

-- Creating table 'Crm_Degres_Sanction'
CREATE TABLE [dbo].[Crm_Degres_Sanction] (
    [id] int IDENTITY(1,1) NOT NULL,
    [du] int  NOT NULL,
    [au] int  NOT NULL,
    [libelle] varchar(245)  NOT NULL,
    [type] varchar(50)  NOT NULL
);
GO

-- Creating table 'Crm_HistoriqueType'
CREATE TABLE [dbo].[Crm_HistoriqueType] (
    [id] int  NOT NULL,
    [NumeroTache] nchar(10)  NULL,
    [IdSanction] int  NULL,
    [IdDegres] int  NULL,
    [DateOperation] datetime  NULL,
    [Type] nchar(10)  NULL,
    [TypePiece] nchar(10)  NULL,
    [NomValidateur] nchar(10)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CodeClient], [id] in table 'ContactClient'
ALTER TABLE [dbo].[ContactClient]
ADD CONSTRAINT [PK_ContactClient]
    PRIMARY KEY CLUSTERED ([CodeClient], [id] ASC);
GO

-- Creating primary key on [NomUtilisateur] in table 'Crm_AccesUtilisateur'
ALTER TABLE [dbo].[Crm_AccesUtilisateur]
ADD CONSTRAINT [PK_Crm_AccesUtilisateur]
    PRIMARY KEY CLUSTERED ([NomUtilisateur] ASC);
GO

-- Creating primary key on [IdContact] in table 'Crm_ContactClient'
ALTER TABLE [dbo].[Crm_ContactClient]
ADD CONSTRAINT [PK_Crm_ContactClient]
    PRIMARY KEY CLUSTERED ([IdContact] ASC);
GO

-- Creating primary key on [idTacheExec] in table 'Crm_CycleExecTache'
ALTER TABLE [dbo].[Crm_CycleExecTache]
ADD CONSTRAINT [PK_Crm_CycleExecTache]
    PRIMARY KEY CLUSTERED ([idTacheExec] ASC);
GO

-- Creating primary key on [NumeroIntervention] in table 'Crm_Intervention'
ALTER TABLE [dbo].[Crm_Intervention]
ADD CONSTRAINT [PK_Crm_Intervention]
    PRIMARY KEY CLUSTERED ([NumeroIntervention] ASC);
GO

-- Creating primary key on [CodeMoyenCommunication] in table 'Crm_MoyenCommunication'
ALTER TABLE [dbo].[Crm_MoyenCommunication]
ADD CONSTRAINT [PK_Crm_MoyenCommunication]
    PRIMARY KEY CLUSTERED ([CodeMoyenCommunication] ASC);
GO

-- Creating primary key on [NumeroNotification] in table 'Crm_Notification'
ALTER TABLE [dbo].[Crm_Notification]
ADD CONSTRAINT [PK_Crm_Notification]
    PRIMARY KEY CLUSTERED ([NumeroNotification] ASC);
GO

-- Creating primary key on [IdOperation], [IdTicket] in table 'Crm_OperationParTicket'
ALTER TABLE [dbo].[Crm_OperationParTicket]
ADD CONSTRAINT [PK_Crm_OperationParTicket]
    PRIMARY KEY CLUSTERED ([IdOperation], [IdTicket] ASC);
GO

-- Creating primary key on [NumeroRapport], [IdPresen] in table 'Crm_PersonnelParRapoort'
ALTER TABLE [dbo].[Crm_PersonnelParRapoort]
ADD CONSTRAINT [PK_Crm_PersonnelParRapoort]
    PRIMARY KEY CLUSTERED ([NumeroRapport], [IdPresen] ASC);
GO

-- Creating primary key on [CodeRespensable], [NumeroIntervention] in table 'Crm_RespensableParIntervention'
ALTER TABLE [dbo].[Crm_RespensableParIntervention]
ADD CONSTRAINT [PK_Crm_RespensableParIntervention]
    PRIMARY KEY CLUSTERED ([CodeRespensable], [NumeroIntervention] ASC);
GO

-- Creating primary key on [NumObjet] in table 'Crm_TableObjet'
ALTER TABLE [dbo].[Crm_TableObjet]
ADD CONSTRAINT [PK_Crm_TableObjet]
    PRIMARY KEY CLUSTERED ([NumObjet] ASC);
GO

-- Creating primary key on [IdtacheExce] in table 'Crm_TacheExcution'
ALTER TABLE [dbo].[Crm_TacheExcution]
ADD CONSTRAINT [PK_Crm_TacheExcution]
    PRIMARY KEY CLUSTERED ([IdtacheExce] ASC);
GO

-- Creating primary key on [NumeroTache], [CodeRespensable] in table 'Crm_TacheLiee'
ALTER TABLE [dbo].[Crm_TacheLiee]
ADD CONSTRAINT [PK_Crm_TacheLiee]
    PRIMARY KEY CLUSTERED ([NumeroTache], [CodeRespensable] ASC);
GO

-- Creating primary key on [NumeroTache] in table 'Crm_TacheReclamation'
ALTER TABLE [dbo].[Crm_TacheReclamation]
ADD CONSTRAINT [PK_Crm_TacheReclamation]
    PRIMARY KEY CLUSTERED ([NumeroTache] ASC);
GO

-- Creating primary key on [IdTicket] in table 'Crm_TicketClient'
ALTER TABLE [dbo].[Crm_TicketClient]
ADD CONSTRAINT [PK_Crm_TicketClient]
    PRIMARY KEY CLUSTERED ([IdTicket] ASC);
GO

-- Creating primary key on [CodeTypeOperation] in table 'Crm_TypeOperation'
ALTER TABLE [dbo].[Crm_TypeOperation]
ADD CONSTRAINT [PK_Crm_TypeOperation]
    PRIMARY KEY CLUSTERED ([CodeTypeOperation] ASC);
GO

-- Creating primary key on [CodeTypeReclamation] in table 'Crm_TypeReclamation'
ALTER TABLE [dbo].[Crm_TypeReclamation]
ADD CONSTRAINT [PK_Crm_TypeReclamation]
    PRIMARY KEY CLUSTERED ([CodeTypeReclamation] ASC);
GO

-- Creating primary key on [NumeroEtat] in table 'EtatCRM'
ALTER TABLE [dbo].[EtatCRM]
ADD CONSTRAINT [PK_EtatCRM]
    PRIMARY KEY CLUSTERED ([NumeroEtat] ASC);
GO

-- Creating primary key on [CodeRespensable] in table 'Respensable'
ALTER TABLE [dbo].[Respensable]
ADD CONSTRAINT [PK_Respensable]
    PRIMARY KEY CLUSTERED ([CodeRespensable] ASC);
GO

-- Creating primary key on [NomUtilisateur] in table 'Utilisateur'
ALTER TABLE [dbo].[Utilisateur]
ADD CONSTRAINT [PK_Utilisateur]
    PRIMARY KEY CLUSTERED ([NomUtilisateur] ASC);
GO

-- Creating primary key on [CodeMotif], [Libelle] in table 'Crm_Motif'
ALTER TABLE [dbo].[Crm_Motif]
ADD CONSTRAINT [PK_Crm_Motif]
    PRIMARY KEY CLUSTERED ([CodeMotif], [Libelle] ASC);
GO

-- Creating primary key on [CodeFonction] in table 'Fonction'
ALTER TABLE [dbo].[Fonction]
ADD CONSTRAINT [PK_Fonction]
    PRIMARY KEY CLUSTERED ([CodeFonction] ASC);
GO

-- Creating primary key on [CodeService] in table 'Service'
ALTER TABLE [dbo].[Service]
ADD CONSTRAINT [PK_Service]
    PRIMARY KEY CLUSTERED ([CodeService] ASC);
GO

-- Creating primary key on [CodePersonne] in table 'Crm_Personne'
ALTER TABLE [dbo].[Crm_Personne]
ADD CONSTRAINT [PK_Crm_Personne]
    PRIMARY KEY CLUSTERED ([CodePersonne] ASC);
GO

-- Creating primary key on [CodeSociete], [NomPiecer] in table 'CompteurPiece'
ALTER TABLE [dbo].[CompteurPiece]
ADD CONSTRAINT [PK_CompteurPiece]
    PRIMARY KEY CLUSTERED ([CodeSociete], [NomPiecer] ASC);
GO

-- Creating primary key on [Id_Reclamation] in table 'Crm_ReclamationClient'
ALTER TABLE [dbo].[Crm_ReclamationClient]
ADD CONSTRAINT [PK_Crm_ReclamationClient]
    PRIMARY KEY CLUSTERED ([Id_Reclamation] ASC);
GO

-- Creating primary key on [CodeClient] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [PK_Client]
    PRIMARY KEY CLUSTERED ([CodeClient] ASC);
GO

-- Creating primary key on [NumeroPVReunion], [CodePersonne] in table 'Crm_PersonneParReunion'
ALTER TABLE [dbo].[Crm_PersonneParReunion]
ADD CONSTRAINT [PK_Crm_PersonneParReunion]
    PRIMARY KEY CLUSTERED ([NumeroPVReunion], [CodePersonne] ASC);
GO

-- Creating primary key on [NumeroPVReunion] in table 'Crm_PVReunion'
ALTER TABLE [dbo].[Crm_PVReunion]
ADD CONSTRAINT [PK_Crm_PVReunion]
    PRIMARY KEY CLUSTERED ([NumeroPVReunion] ASC);
GO

-- Creating primary key on [NumeroRapport] in table 'Crm_Rapport'
ALTER TABLE [dbo].[Crm_Rapport]
ADD CONSTRAINT [PK_Crm_Rapport]
    PRIMARY KEY CLUSTERED ([NumeroRapport] ASC);
GO

-- Creating primary key on [NumeroReclamation], [NomUtilisateur] in table 'Crm_PersonneConcerner'
ALTER TABLE [dbo].[Crm_PersonneConcerner]
ADD CONSTRAINT [PK_Crm_PersonneConcerner]
    PRIMARY KEY CLUSTERED ([NumeroReclamation], [NomUtilisateur] ASC);
GO

-- Creating primary key on [Id_Planning] in table 'Crm_Planning'
ALTER TABLE [dbo].[Crm_Planning]
ADD CONSTRAINT [PK_Crm_Planning]
    PRIMARY KEY CLUSTERED ([Id_Planning] ASC);
GO

-- Creating primary key on [CodeRespensable], [Id_Planning] in table 'Crm_Planning_Responsable'
ALTER TABLE [dbo].[Crm_Planning_Responsable]
ADD CONSTRAINT [PK_Crm_Planning_Responsable]
    PRIMARY KEY CLUSTERED ([CodeRespensable], [Id_Planning] ASC);
GO

-- Creating primary key on [Id_Ligne] in table 'Crm_PlanningTime'
ALTER TABLE [dbo].[Crm_PlanningTime]
ADD CONSTRAINT [PK_Crm_PlanningTime]
    PRIMARY KEY CLUSTERED ([Id_Ligne] ASC);
GO

-- Creating primary key on [CodeClient] in table 'ClientCRM'
ALTER TABLE [dbo].[ClientCRM]
ADD CONSTRAINT [PK_ClientCRM]
    PRIMARY KEY CLUSTERED ([CodeClient] ASC);
GO

-- Creating primary key on [Nature] in table 'Crm_TacheNature'
ALTER TABLE [dbo].[Crm_TacheNature]
ADD CONSTRAINT [PK_Crm_TacheNature]
    PRIMARY KEY CLUSTERED ([Nature] ASC);
GO

-- Creating primary key on [CodeModeTache] in table 'crm_ModeTache'
ALTER TABLE [dbo].[crm_ModeTache]
ADD CONSTRAINT [PK_crm_ModeTache]
    PRIMARY KEY CLUSTERED ([CodeModeTache] ASC);
GO

-- Creating primary key on [IdNatureType] in table 'Crm_ModuleNature'
ALTER TABLE [dbo].[Crm_ModuleNature]
ADD CONSTRAINT [PK_Crm_ModuleNature]
    PRIMARY KEY CLUSTERED ([IdNatureType] ASC);
GO

-- Creating primary key on [NumeroContratClient] in table 'ContratClient'
ALTER TABLE [dbo].[ContratClient]
ADD CONSTRAINT [PK_ContratClient]
    PRIMARY KEY CLUSTERED ([NumeroContratClient] ASC);
GO

-- Creating primary key on [NumeroTacheCommercial], [NumeroTacheSoft_Tecnique] in table 'Crm_TacheCommercialSoft'
ALTER TABLE [dbo].[Crm_TacheCommercialSoft]
ADD CONSTRAINT [PK_Crm_TacheCommercialSoft]
    PRIMARY KEY CLUSTERED ([NumeroTacheCommercial], [NumeroTacheSoft_Tecnique] ASC);
GO

-- Creating primary key on [NumeroEmploiTemp], [CodeGroupe] in table 'EmploiDuTemp'
ALTER TABLE [dbo].[EmploiDuTemp]
ADD CONSTRAINT [PK_EmploiDuTemp]
    PRIMARY KEY CLUSTERED ([NumeroEmploiTemp], [CodeGroupe] ASC);
GO

-- Creating primary key on [MatriculePersonnel] in table 'PersonnelGRH'
ALTER TABLE [dbo].[PersonnelGRH]
ADD CONSTRAINT [PK_PersonnelGRH]
    PRIMARY KEY CLUSTERED ([MatriculePersonnel] ASC);
GO

-- Creating primary key on [CodeClientParent] in table 'Crm_ClientParent'
ALTER TABLE [dbo].[Crm_ClientParent]
ADD CONSTRAINT [PK_Crm_ClientParent]
    PRIMARY KEY CLUSTERED ([CodeClientParent] ASC);
GO

-- Creating primary key on [CodeClientParent], [CodeClientFils] in table 'Crm_ClientParentAssociation'
ALTER TABLE [dbo].[Crm_ClientParentAssociation]
ADD CONSTRAINT [PK_Crm_ClientParentAssociation]
    PRIMARY KEY CLUSTERED ([CodeClientParent], [CodeClientFils] ASC);
GO

-- Creating primary key on [CodeEquipe] in table 'Crm_Equipe'
ALTER TABLE [dbo].[Crm_Equipe]
ADD CONSTRAINT [PK_Crm_Equipe]
    PRIMARY KEY CLUSTERED ([CodeEquipe] ASC);
GO

-- Creating primary key on [CodeTypeTache] in table 'Crm_TypeTache'
ALTER TABLE [dbo].[Crm_TypeTache]
ADD CONSTRAINT [PK_Crm_TypeTache]
    PRIMARY KEY CLUSTERED ([CodeTypeTache] ASC);
GO

-- Creating primary key on [IdSanction] in table 'Crm_Sanction'
ALTER TABLE [dbo].[Crm_Sanction]
ADD CONSTRAINT [PK_Crm_Sanction]
    PRIMARY KEY CLUSTERED ([IdSanction] ASC);
GO

-- Creating primary key on [id] in table 'Crm_Degres_Sanction'
ALTER TABLE [dbo].[Crm_Degres_Sanction]
ADD CONSTRAINT [PK_Crm_Degres_Sanction]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Crm_HistoriqueType'
ALTER TABLE [dbo].[Crm_HistoriqueType]
ADD CONSTRAINT [PK_Crm_HistoriqueType]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CodeFonction] in table 'Utilisateur'
ALTER TABLE [dbo].[Utilisateur]
ADD CONSTRAINT [FK_Utilisateur_Fonction]
    FOREIGN KEY ([CodeFonction])
    REFERENCES [dbo].[Fonction]
        ([CodeFonction])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Utilisateur_Fonction'
CREATE INDEX [IX_FK_Utilisateur_Fonction]
ON [dbo].[Utilisateur]
    ([CodeFonction]);
GO

-- Creating foreign key on [Id_Plannig] in table 'Crm_PlanningTime'
ALTER TABLE [dbo].[Crm_PlanningTime]
ADD CONSTRAINT [FK_Crm_PlanningTime_Crm_Planning]
    FOREIGN KEY ([Id_Plannig])
    REFERENCES [dbo].[Crm_Planning]
        ([Id_Planning])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Crm_PlanningTime_Crm_Planning'
CREATE INDEX [IX_FK_Crm_PlanningTime_Crm_Planning]
ON [dbo].[Crm_PlanningTime]
    ([Id_Plannig]);
GO

-- Creating foreign key on [Nature] in table 'Crm_ModuleNature'
ALTER TABLE [dbo].[Crm_ModuleNature]
ADD CONSTRAINT [FK_Crm_ModuleNature_Crm_TacheNature]
    FOREIGN KEY ([Nature])
    REFERENCES [dbo].[Crm_TacheNature]
        ([Nature])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Crm_ModuleNature_Crm_TacheNature'
CREATE INDEX [IX_FK_Crm_ModuleNature_Crm_TacheNature]
ON [dbo].[Crm_ModuleNature]
    ([Nature]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------