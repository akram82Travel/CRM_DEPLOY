/*=========================================================================================
    File Name: kanban.js
    Description: kanban plugin
    ----------------------------------------------------------------------------------------
    Item Name: Modern KANABAN
    Author: AKRAM HAMDI
   
==========================================================================================*/

$(document).ready(function() {
function time_convert(num)
 { 
  const hours = Math.floor(num / 60);  
  const minutes = num % 60;
  return `${hours}:${minutes}`;         
}
    if($("#kanban-wrapper").length >0){
        var kanban_curr_el, kanban_curr_item_id, kanban_item_title, kanban_data, kanban_item, kanban_users, kanban_curr_item_reclamation, kanban_curr_item_reclamation_status;

    // appel de la liste des taches

    $("#spinner").hide();
    var ajaxResult = null;
    ajaxResulta = [];
    // set tache non commencer array

    ajaxResultCommencer = [];
    // set tache en execution

    ajaxResultEnExecution = [];
    // set tache en pause

    ajaxResultEnPause = [];

    // set tache a valider

    ajaxResultAValider = [];

    // set tache a tester

    ajaxResultAtester = [];

    // set tache terminee

    ajaxResultTerminee = [];

$("#loadingScreen").hide();
function removeDuplicates(arr) {
        keys = ['NumeroTache'],
    filtered = arr.filter(
        (s => o => 
            (k => !s.has(k) && s.add(k))
            (keys.map(k => o[k]).join('|'))
        )
        (new Set)
    );
        return filtered;
    }
        $("#btnFilter").click(

            function () {
                
                ajaxResulta = [];
                // set tache non commencer array

                ajaxResultCommencer = [];
                // set tache en execution

                ajaxResultEnExecution = [];
                // set tache en pause

                ajaxResultEnPause = [];

                // set tache a valider

                ajaxResultAValider = [];

                // set tache a tester

                ajaxResultAtester = [];

                // set tache terminee

                ajaxResultTerminee = [];
            $("#kanban-wrapper").html("");
                
            // set params for ajax
            dataReclamation = {
                CodeRespensable: $("#CodeRespensable").val(),
                CodeClient: $("#CodeClient").val(),
                NumeroTache: $("#NumeroTache").val(),
                NomPlanificateur: $("#NomPlanificateur").val(),
                NomValidateur: $("#NomValidateur").val(),
                NomTesteur: $("#NomTesteur").val(),
                DateCreation: $("#d1").val(),
                DateCreationFin: $("#d2").val(),
                DescriptionTache: $("#DescriptionTache").val(),
                Type: userRole,
                NumPiece: $("#NumPiece").val(),
                SelectedresponsableId: $("#SelectedresponsableId").val(),
            };

            // set ajax
            $.ajax({
                method: "POST",
                async: true,
                contentType: 'application/json',
                dataType: 'json',
                url: "/Crm_TacheReclamation/listeTache",
                data: JSON.stringify(dataReclamation),
beforeSend: function() {
        
$("#loadingScreen").show();
    },
success: function(data) {
       
$("#loadingScreen").hide();
    },
            })
                .done(function (msg) {
                  
                    var array = removeDuplicates(JSON.parse(msg));
                    //console.log(array);
                    array.forEach(function (object) {
                    if(object.Nature != "C"){
                    // get tache non commencer
                        if (object.Status === "E11" || object.Status === "E10") {
                            ajaxResultCommencer.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: "0",
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }


                        // get tach en execution
                        if (object.Status === "E07") {
                            ajaxResultEnExecution.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: object.DureeReel,
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }

                        // get tach en pause
                        if (object.Status === "E06") {
                            ajaxResultEnPause.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: object.DureeReel,
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }

                        // get tach A Valider
                        if (object.Status === "E04") {
                            ajaxResultAValider.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: object.DureeReel,
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }

                        // get tach A Tester
                        if (object.Status === "E03" || object.codeResponsable == userName) {
                            ajaxResultAtester.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: object.DureeReel,
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }
                        // get tach terminee
                        if (object.Status === "E01") {
                            ajaxResultTerminee.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: "",
                                border: "success",
                                Status: object.Status,
                                client: object.RaisonSociale,
                                duree: object.Duree,
                                DureeReel: object.DureeReel,
                                rapport: object.NumDossier,
                                dueDate: object.DateCreation,
                                attachment: object.Nom,
                                numreclamation: object.NumReclamation,
                                numreclamationstatus: object.StatusReclamation,
                                degres: object.Degres,
                            });
                        }


}
                         });
                    let nonCommencer = {};
                    ajaxResult = [];
                    ajaxResult.push({
                        id: "kanban-board-1",
                        title: "Non Commencé",
                        dragTo: ['kanban-board-2'],
                        item: ajaxResultCommencer
                    });

                    if (userRole == "01" || userRole == "02") {
                        ajaxResult.push({
                            id: "kanban-board-2",
                            title: "En Exécution",
                            dragTo: ['kanban-board-3','kanban-board-4','kanban-board-6'],
                            item: ajaxResultEnExecution
                        }, {
                            id: "kanban-board-3",
                            title: "En Pause",
                            dragTo: ['kanban-board-2', 'kanban-board-4'],
                            item: ajaxResultEnPause,
                        }, {
                            id: "kanban-board-4",
                            title: "A valider",
                            dragTo: ['kanban-board-3','kanban-board-5','kanban-board-6'],
                            item: ajaxResultAValider,
                        }, {
                            id: "kanban-board-5",
                            title: "A tester",
                            dragTo: ['kanban-board-4', 'kanban-board-6'],
                            item: ajaxResultAtester,
                        }, {
                            id: "kanban-board-6",
                            title: "Terminé",
                            dragTo: ['kanban-board-4', 'kanban-board-5','kanban-board-6'],
                            item: ajaxResultTerminee,
                        });
                    }
                    if (userRole == "03") {
                        ajaxResult.push({
                            id: "kanban-board-2",
                            title: "En Exécution",
                            dragTo: ['kanban-board-3', 'kanban-board-4','kanban-board-6'],
                            item: ajaxResultEnExecution
                        }, {
                            id: "kanban-board-3",
                            title: "En Pause",
                            dragTo: ['kanban-board-2', 'kanban-board-4'],
                            item: ajaxResultEnPause,
                        }, {
                            id: "kanban-board-4",
                            title: "A valider",
                            dragTo: ['kanban-board-5'],
                            item: ajaxResultAValider,
                        }, {
                            id: "kanban-board-5",
                            title: "A tester",
                            dragTo: ['kanban-board-4', 'kanban-board-6'],
                            item: ajaxResultAtester,
                        });
                    }



                    // Kanban Board and Item Data passed by json
                    var kanban_board_data = ajaxResult;

                    // Kanban Board
                    var KanbanExample = new jKanban({
                        element: "#kanban-wrapper", // selector of the kanban container
                        buttonContent: "", // text or html content of the board button
                        dragBoards: false,
                        itemHandleOptions: {
                            enabled: false, // if board item handle is enabled or not
                            handleClass: "item_handle", // css class for your custom item handle
                            customCssHandler: "drag_handler", // when customHandler is undefined, jKanban will use this property to set main handler class
                            customCssIconHandler: "drag_handler_icon", // when customHandler is undefined, jKanban will use this property to set main icon handler class. If you want, you can use font icon libraries here
                            customHandler: "<span class='item_handle'>+</span> %key% " // your entirely customized handler. Use %title% to position item title 
                        }, // any key's value included in item collection can be replaced with %key%

                        // click on current kanban-item
                        click: function (el) {

                            // kanban-overlay and sidebar display block on click of kanban-item
                            $(".kanban-overlay").addClass("show");
                            $(".kanban-sidebar").addClass("show");
                            // Set el to var kanban_curr_el, use this variable when updating title
                            kanban_curr_el = el;
                            // Extract  the kan ban item & id and set it to respective vars

                             kanban_item_title = $(el).contents()[0].data;
                            kanban_item_description = $(el).contents()[1].data;
                            kanban_curr_item_id = $(el).attr("data-eid");
                            kanban_curr_item_comment = $(el).attr("data-comment");
                            kanban_curr_item_date = $(el).attr("data-duedate");
                            kanban_curr_item_client = $(el).attr("data-client");
                            kanban_curr_item_duree = $(el).attr("data-duree");
                            kanban_curr_item_DureeReel = $(el).attr("data-dureereel");
                            kanban_curr_item_Status = $(el).attr("data-status");
                            kanban_curr_item_rapport = $(el).attr("data-rapport");
                            kanban_curr_item_reclamation = $(el).attr("data-numreclamation");
                            kanban_curr_item_reclamation_status = $(el).attr("data-numreclamationstatus");
                            // set edit title
                            $(".edit-kanban-item .edit-kanban-item-title").html("<span class='title-kanban'>" + kanban_item_title + "</span>");
                            $(".edit-kanban-item .edit-kanban-item-date").val(kanban_curr_item_date);
                            $(".edit-kanban-item .edit-kanban-item-comment").html(kanban_curr_item_comment);
                            $(".edit-kanban-item .edit-kanban-item-client").html(kanban_curr_item_client);
                            $(".edit-kanban-item .edit-kanban-item-duree").html(kanban_curr_item_duree);
                            $(".edit-kanban-item .edit-kanban-item-DureeReel").html(time_convert(kanban_curr_item_DureeReel));
                            $(".edit-kanban-item .edit-kanban-item-Status").html(time_convert(kanban_curr_item_Status));
                            $(".edit-kanban-item .edit-kanban-item-rapport").html(kanban_curr_item_rapport);
                            $(".edit-kanban-item .edit-kanban-item-reclamation").html(kanban_curr_item_reclamation);
                            $(".edit-kanban-item .edit-kanban-item-reclamationStatus").html(kanban_curr_item_reclamation_status);
                            $(".kanban_curr_item_id").val(kanban_curr_item_id);
                            $(".kanban_curr_item_reclamation").val(kanban_curr_item_reclamation);
                            $(".kanban_curr_item_reclamation_status").val(kanban_curr_item_reclamation_status);
                            $(".kanban_item_id").html(kanban_curr_item_id);

                        },
                        dropEl: function (el, target, source, sibling) {
                            console.log(target);
                            //KanbanExample.replaceElement(target.getAttribute("data-eid"), source)
                            // KanbanExample.replaceElement(source, target)
                            // get element fraged
                            // console.log(el.getAttribute("data-duedate"))
                            console.log(el.getAttribute("data-eid"))
                            // from etat
                            //console.log(source.parentNode.dataset.id);
                            console.log(source.parentNode.dataset.order);

                            // to etat
                            //console.log(target.parentNode.dataset.id);
                            console.log(target.parentNode.dataset.order);

                            dataReclamation = {
                                Status: target.parentNode.dataset.order,
                                NumeroTache: el.getAttribute("data-eid"),
                                CodeRespensable: "",
                                NomPlanificateur: "",
                                CodeClient: "",
                                ValiderPar: "",
                                UtilisateurCreateur: userName, // session connecter
                                Type: userRole, // role connecter
                            };
                            $.ajax({
                                method: "POST",
                                async: false,
                                contentType: 'application/json',
                                dataType: 'json',
                                url: "/Crm_TacheReclamation/UpdateStateTache",
                                data: JSON.stringify(dataReclamation)
                            })
                                .done(function (msg) { });
                        },
                        addItemButton: false, // add a button to board for easy item creation
                        boards: kanban_board_data // data passed from defined variable
                    });

                    // Add html for Custom Data-attribute to Kanban item
                    var board_item_id, board_item_el;
                    // Kanban board loop


                    for (kanban_data in kanban_board_data) {
                        // Kanban board items loop
                        for (kanban_item in kanban_board_data[kanban_data].item) {
                            
                            var board_item_details = kanban_board_data[kanban_data].item[kanban_item]; // set item details
                            board_item_id = $(board_item_details).attr("id"); // set 'id' attribute of kanban-item

                            (board_item_el = KanbanExample.findElement(board_item_id)), // find element of kanban-item by ID
                                (board_item_users = board_item_client = board_item_dueDate = board_item_comment = board_item_attachment = board_item_duree = board_item_DureeReel = board_item_image = board_item_badge = board_item_reclamation = board_item_degres =
                                    " ");

                            // check if users are defined or not and loop it for getting value from user's array
                            if (typeof $(board_item_el).attr("data-users") !== "undefined") {
                                for (kanban_users in kanban_board_data[kanban_data].item[kanban_item].users) {
                                    board_item_users +=
                                        '<li class="avatar pull-up my-0">' +
                                        '<img class="media-object" src=" ' +
                                        kanban_board_data[kanban_data].item[kanban_item].users[kanban_users] +
                                        '" alt="Avatar" height="18" width="18">' +
                                        "</li>";
                                }
                            }
                            // check if dueDate is defined or not
                            if (typeof $(board_item_el).attr("data-dueDate") !== "undefined") {
                                board_item_dueDate =
                                    '<div class="kanban-due-date mr-25">' +
                                    '<i class="ft-clock font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-dueDate").slice(0, -9) +
                                    "</span>" +
                                    "</div>";
                            }
                            // check if reclamation is defined or not
                            if (typeof $(board_item_el).attr("data-numreclamation") !== "undefined" && $(board_item_el).attr("data-numreclamation") != "") {
                                board_item_reclamation = '<div class="kanban-reclamation mr-25"><i class="ft-alert-triangle font-size-small mr-25"></i> ' + $(board_item_el).attr("data-numreclamationstatus") +'</div>';
                            }
                            // check if reclamation is defined or not
                            if (typeof $(board_item_el).attr("data-degres") !== "undefined" && $(board_item_el).attr("data-degres") == 1) {
                                
                                board_item_degres =
                                    '<div class="kanban-degres mr-25"><i class="ft-alert-circle font-size-small mr-25"></i></div>';
                            }
                            // check if comment is defined or not
                            if (typeof $(board_item_el).attr("data-comment") !== "undefined") {
                                board_item_comment =
                                    '<div class="kanban-comment mr-50">' +
                                    '<i class="ft-message-square font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-comment") +
                                    "</span>" +
                                    "</div>";
                            }
                            // check if attachment is defined or not
                            if (typeof $(board_item_el).attr("data-attachment") !== "undefined") {
                                board_item_attachment =
                                    '<div class="kanban-attachment">' +
                                    '<i class="ft-user font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-attachment") +
                                    "</span>" +
                                    "</div>";
                            }
                // check if duree is defined or not
                    if (typeof $(board_item_el).attr("data-duree") !== "undefined") {
                        board_item_duree =
                            '<div class="kanban-duree">' +
                            '<i class="ft-clock font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                             $(board_item_el).attr("data-duree") + " / <span class='text-success'>" + time_convert($(board_item_el).attr("data-dureereel"))+ "</span>" +
                            "</span>" +
                            "</div>";
                    }
                // check if DureeReel is defined or not
                    if (typeof $(board_item_el).attr("data-dureereel") !== "undefined") {
                        board_item_DureeReel =
                            '<div class="kanban-DureeReel">' +
                            '<i class="ft-clock font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-dureereel") +
                            "</span>" +
                            "</div>";
                    }

                    // check if client is defined or not
                    if (typeof $(board_item_el).attr("data-client") !== "undefined") {
                        board_item_client =
                            '<div class="kanban-client">' +
                            '<i class="ft-edit font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-client") +
                            "</span>" +
                            "</div>";
                    }
                            // check if Image is defined or not
                            if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                                board_item_image =
                                    '<div class="kanban-image mb-1">' +
                                    '<img class="img-fluid" src=" ' +
                                    kanban_board_data[kanban_data].item[kanban_item].image +
                                    '" alt="kanban-image">';
                                ("</div>");
                            } 

                            // check if Badge is defined or not
                            if (typeof $(board_item_el).attr("data-badgeContent") !== "undefined") {
                                board_item_badge =
                                    '<div class="kanban-badge">' +
                                    '<div class="avatar bg-' +
                                    kanban_board_data[kanban_data].item[kanban_item].badgeColor +
                                    ' font-size-small font-weight-bold">' +
                                    kanban_board_data[kanban_data].item[kanban_item].badgeContent +
                                    "</div>";
                                ("</div>");
                            }
                            // add custom 'kanban-footer'
                            if (
                                typeof (
                                    $(board_item_el).attr("data-dueDate") ||
                                    $(board_item_el).attr("data-comment") ||
                                    $(board_item_el).attr("data-users") ||
                                    $(board_item_el).attr("data-attachment")
                                ) !== "undefined"
                            ) {
                                $(board_item_el).append(
                                    '<hr/>' + '<div class="kanban-footer-left d-flex">' + board_item_degres + board_item_reclamation + board_item_dueDate + board_item_duree +  "</div>" + '<hr/>' + board_item_comment +'<hr><div class="kanban-footer d-flex justify-content-between mt-1">' +
                            '<div class="kanban-footer-left d-flex">' +
                            
                            board_item_attachment + board_item_client + 
                            "</div>" +
                            '<div class="kanban-footer-right">' +
                            '<div class="kanban-users">'  + 
                            board_item_badge +
                            '<ul class="list-unstyled users-list cursor-pointer m-0 d-flex align-items-center">' +
                            board_item_users +
                            "</ul>" +
                            "</div>" +
                            "</div>" +
                            "</div>"
                        );
                            }
                            // add Image prepend to 'kanban-Item'
                            if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                                $(board_item_el).prepend(board_item_image);
                            }
                        }
                    }
                    kanban_board_data = [];
if(ajaxResultCommencer.length>0){
$("[data-id~='kanban-board-1'] .kanban-board-header").css("background-color", "orange");

}

                });
  
            /// END SET AJAX****/
        }
    );

        dataReclamation = {
            CodeRespensable: "",
            NomPlanificateur: "",
            CodeClient: "",
            NomValidateur: "",
            DateCreation: $("#d1").val(),
            DateCreationFin: $("#d2").val(),
            NomTesteur: "",
            UtilisateurCreateur: userName, // session connecter
            DescriptionTache: "",
            Type: userRole,
            NumPiece: "",
            SelectedresponsableId: JSON.stringify({}),
        };

    
    $.ajax({
            method: "POST",
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            url: "/Crm_TacheReclamation/listeTache",
            data: JSON.stringify(dataReclamation)
        })
        .done(function(msg) {

            var array = removeDuplicates(JSON.parse(msg));
            console.log(array);
            array.forEach(function(object) {
            if(object.Nature != "C"){
            // get tache non commencer
                if (object.Status === "E11" || object.Status === "E10") {
                    ajaxResultCommencer.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: "0",
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }


                // get tach en execution
                if (object.Status === "E07") {
                    ajaxResultEnExecution.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: object.DureeReel,
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }

                // get tach en pause
                if (object.Status === "E06") {
                    ajaxResultEnPause.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: object.DureeReel,
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }

                // get tach A Valider
                if (object.Status === "E04") {
                    ajaxResultAValider.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: object.DureeReel,
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }
               
                // get tach A Tester
                if (object.Status === "E03" || object.codeResponsable == userName) {
                    ajaxResultAtester.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: object.DureeReel,
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }
                // get tach terminee
                if (object.Status === "E01") {
                    ajaxResultTerminee.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: "",
                        border: "success",
                        client: object.RaisonSociale,
                        duree: object.Duree,
                        DureeReel: object.DureeReel,
                        Status: object.Status,
                        rapport: object.NumDossier,
                        dueDate: object.DateCreation,
                        attachment: object.Nom,
                        numreclamation: object.NumReclamation,
                        numreclamationstatus: object.StatusReclamation,
                        degres: object.Degres,
                    });
                }

           
}
                 });

            let nonCommencer = {};
            ajaxResult = [];
            ajaxResult.push({
                    id: "kanban-board-1",
                    title: "Non Commencé",
                    dragTo: ['kanban-board-2'],
                    item: ajaxResultCommencer
                });
            if(userRole == "01" || userRole == "02"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3','kanban-board-4','kanban-board-6'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-3','kanban-board-5','kanban-board-6'],
                item: ajaxResultAValider,
            }, {
                id: "kanban-board-5",
                title: "A tester",
                dragTo: ['kanban-board-4', 'kanban-board-6'],
                item: ajaxResultAtester,
            }, {
                id: "kanban-board-6",
                title: "Terminé",
                dragTo: ['kanban-board-4', 'kanban-board-5', 'kanban-board-6'],
                item: ajaxResultTerminee,
            });
            }
            if(userRole == "03"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3', 'kanban-board-4', 'kanban-board-6'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-5'],
                item: ajaxResultAValider,
            }, {
                id: "kanban-board-5",
                title: "A tester",
                dragTo: ['kanban-board-4', 'kanban-board-6'],
                item: ajaxResultAtester,
            });
            }
            


            // Kanban Board and Item Data passed by json
            var kanban_board_data = ajaxResult;

            // Kanban Board
            var KanbanExample = new jKanban({
                element: "#kanban-wrapper", // selector of the kanban container
                buttonContent: "", // text or html content of the board button
                dragBoards       : false,
                itemHandleOptions: {
                    enabled: false, // if board item handle is enabled or not
                    handleClass: "item_handle", // css class for your custom item handle
                    customCssHandler: "drag_handler", // when customHandler is undefined, jKanban will use this property to set main handler class
                    customCssIconHandler: "drag_handler_icon", // when customHandler is undefined, jKanban will use this property to set main icon handler class. If you want, you can use font icon libraries here
                    customHandler: "<span class='item_handle'>+</span> %key% " // your entirely customized handler. Use %title% to position item title 
                }, // any key's value included in item collection can be replaced with %key%

                // click on current kanban-item
                click: function(el) {
      
                            // kanban-overlay and sidebar display block on click of kanban-item
                            $(".kanban-overlay").addClass("show");
                            $(".kanban-sidebar").addClass("show");
                            // Set el to var kanban_curr_el, use this variable when updating title
                            kanban_curr_el = el;
                            // Extract  the kan ban item & id and set it to respective vars
                    
                            kanban_item_title = $(el).contents()[0].data;
                            kanban_item_description = $(el).contents()[1].data;
                            kanban_curr_item_id = $(el).attr("data-eid");
                            kanban_curr_item_comment = $(el).attr("data-comment");
                            kanban_curr_item_date = $(el).attr("data-duedate");
                            kanban_curr_item_client = $(el).attr("data-client");
                            kanban_curr_item_duree = $(el).attr("data-duree");
                            kanban_curr_item_DureeReel = $(el).attr("data-dureereel");
                            kanban_curr_item_Status = $(el).attr("data-status");
                            kanban_curr_item_rapport = $(el).attr("data-rapport");
                            kanban_curr_item_reclamation = $(el).attr("data-numreclamation");
                            kanban_curr_item_reclamation_status = $(el).attr("data-numreclamationstatus");
                            // set edit title
                            $(".edit-kanban-item .edit-kanban-item-title").html("<span class='title-kanban'>" + kanban_item_title + "</span>");
                            $(".edit-kanban-item .edit-kanban-item-date").val(kanban_curr_item_date);
                            $(".edit-kanban-item .edit-kanban-item-comment").html(kanban_curr_item_comment);
                            $(".edit-kanban-item .edit-kanban-item-client").html(kanban_curr_item_client);
                            $(".edit-kanban-item .edit-kanban-item-duree").html(kanban_curr_item_duree);
                            $(".edit-kanban-item .edit-kanban-item-DureeReel").html(time_convert(kanban_curr_item_DureeReel));
                            $(".edit-kanban-item .edit-kanban-item-Status").html(time_convert(kanban_curr_item_Status));
                            $(".edit-kanban-item .edit-kanban-item-rapport").html(kanban_curr_item_rapport);
                            $(".edit-kanban-item .edit-kanban-item-reclamation").html(kanban_curr_item_reclamation);
                            $(".edit-kanban-item .edit-kanban-item-reclamationStatus").html(kanban_curr_item_reclamation_status);

                            $(".kanban_curr_item_id").val(kanban_curr_item_id);
                            $(".kanban_curr_item_reclamation").val(kanban_curr_item_reclamation);
                            $(".kanban_curr_item_reclamation_status").val(kanban_curr_item_reclamation_status);
                            $(".kanban_item_id").html(kanban_curr_item_id);

                        },
                dropEl: function(el, target, source, sibling) {
                    console.log(target);
                    //KanbanExample.replaceElement(target.getAttribute("data-eid"), source)
                    // KanbanExample.replaceElement(source, target)
                    // get element fraged
                    // console.log(el.getAttribute("data-duedate"))
                    console.log(el.getAttribute("data-eid"))
                        // from etat
                        //console.log(source.parentNode.dataset.id);
                    console.log(source.parentNode.dataset.order);

                    // to etat
                    //console.log(target.parentNode.dataset.id);
                    console.log(target.parentNode.dataset.order);

                    dataReclamation = {
                        Status: target.parentNode.dataset.order,
                        NumeroTache: el.getAttribute("data-eid"),
                        CodeRespensable: "",
                        NomPlanificateur: "",
                        CodeClient: "",
                        ValiderPar: "",
                        UtilisateurCreateur: userName, // session connecter
                        Type:userRole, // role connecter
    };
                    $.ajax({
                            method: "POST",
                            async: false,
                            contentType: 'application/json',
                            dataType: 'json',
                            url: "/Crm_TacheReclamation/UpdateStateTache",
                            data: JSON.stringify(dataReclamation)
                        })
                        .done(function(msg) {});
                },
                addItemButton: false, // add a button to board for easy item creation
                boards: kanban_board_data // data passed from defined variable
            });

            // Add html for Custom Data-attribute to Kanban item
            var board_item_id, board_item_el;
            // Kanban board loop
            
           
            for (kanban_data in kanban_board_data) {
                // Kanban board items loop
                for (kanban_item in kanban_board_data[kanban_data].item) {
                    var texttimer = "";
                    var board_item_details = kanban_board_data[kanban_data].item[kanban_item]; // set item details
                    board_item_id = $(board_item_details).attr("id"); // set 'id' attribute of kanban-item
                    
                    (board_item_el = KanbanExample.findElement(board_item_id)), // find element of kanban-item by ID
                        (board_item_users = board_item_client = board_item_dueDate = board_item_comment = board_item_attachment = board_item_duree = board_item_DureeReel = board_item_image = board_item_badge = board_item_reclamation = board_item_degres =
                        " ");



                    if ($(board_item_el).attr("data-status") == "E07") {

                        const targetDate = new Date("2023-08-29T12:00:00"); // Replace with your desired datetime
                        var timer = "";
                        var idSpann = "start-datetime" + board_item_id;
                        timer = '<span id="' + idSpann + '">00:00</span>';
                        console.log(timer);
                        function updateTimer(NumTache) {
                            var idSpan = "start-datetime" + NumTache;
                            const now = new Date();
                            const timeDifference = targetDate - now;

                            if (timeDifference <= 0) {
                                $(idSpan).html("Timer Expired!");
                                clearInterval(timerInterval);
                            } else {
                                const days = Math.floor(timeDifference / (1000 * 60 * 60 * 24));
                                const hours = Math.floor((timeDifference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                                const minutes = Math.floor((timeDifference % (1000 * 60 * 60)) / (1000 * 60));
                                const seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);

                                $(idSpan).html(`${days}d ${hours}h ${minutes}m ${seconds}s`);
                            }
                          
                        }


                        // Update the timer every second
                        const timerInterval = setInterval(updateTimer, 1000);


                        updateTimer(board_item_id);
                        
                    }



                    // check if users are defined or not and loop it for getting value from user's array
                    if (typeof $(board_item_el).attr("data-users") !== "undefined") {
                        for (kanban_users in kanban_board_data[kanban_data].item[kanban_item].users) {
                            board_item_users +=
                                '<li class="avatar pull-up my-0">' +
                                '<img class="media-object" src=" ' +
                                kanban_board_data[kanban_data].item[kanban_item].users[kanban_users] +
                                '" alt="Avatar" height="18" width="18">' +
                                "</li>";
                        }
                    }
                    // check if dueDate is defined or not
                    if (typeof $(board_item_el).attr("data-dueDate") !== "undefined") {
                        board_item_dueDate =
                            '<div class="kanban-due-date mr-25">' +
                            '<i class="ft-calendar  font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-dueDate").slice(0, -9) +
                            "</span>" +
                            "</div>";
                    }
                    // check if reclamation is defined or not
                    if (typeof $(board_item_el).attr("data-numreclamation") !== "undefined" && $(board_item_el).attr("data-numreclamation") != "") {
                        board_item_reclamation = '<div class="kanban-reclamation mr-25"><i class="ft-alert-triangle font-size-small mr-25"></i> ' + $(board_item_el).attr("data-numreclamationstatus") + '</div>';
                    }
                    // check if reclamation is defined or not
                    if ($(board_item_el).attr("data-degres") == "1") {
                        
                        board_item_degres = '<div class="kanban-degres mr-25"><i class="ft-alert-circle font-size-small mr-25"></i></div>';
                    }
                    // check if comment is defined or not
                    if (typeof $(board_item_el).attr("data-comment") !== "undefined") {
                        board_item_comment =
                            '<div class="kanban-comment mr-50">' +
                            '<i class="ft-message-square font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-comment") +
                            "</span>" +
                            "</div>";
                    }
                    // check if attachment is defined or not
                    if (typeof $(board_item_el).attr("data-attachment") !== "undefined") {
                        board_item_attachment =
                            '<div class="kanban-attachment">' +
                            '<i class="ft-user font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-attachment") +
                            "</span>" +
                            "</div>";
                    }
                // check if duree is defined or not
                    if (typeof $(board_item_el).attr("data-duree") !== "undefined") {
                        board_item_duree =
                            '<div class="kanban-duree">' +
                            '<i class="ft-clock font-size-small mr-25"></i>' +
                        '<span class="font-size-small">' + texttimer  + 
                            " <span class='text-info'>"+$(board_item_el).attr("data-duree") + "</span> / <span class='text-success'>" + time_convert($(board_item_el).attr("data-dureereel"))+ "</span>" +
                            "</span>" +
                            "</div>";
                    }
                // check if DureeReel is defined or not
                    if (typeof $(board_item_el).attr("data-dureereel") !== "undefined") {
                        board_item_DureeReel =
                            '<div class="kanban-DureeReel">' +
                            '<i class="ft-clock font-size-small mr-25"></i>' +
                        '<span class="font-size-small">' +
                            $(board_item_el).attr("data-dureereel") +
                            "</span>" +
                            "</div>";
                    }
                        // check if client is defined or not
                    if (typeof $(board_item_el).attr("data-client") !== "undefined") {
                        board_item_client =
                            '<div class="kanban-client">' +
                            '<i class="ft-edit font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-client") +
                            "</span>" +
                            "</div>";
                    }
                    // check if Image is defined or not
                    if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                        board_item_image =
                            '<div class="kanban-image mb-1">' +
                            '<img class="img-fluid" src=" ' +
                            kanban_board_data[kanban_data].item[kanban_item].image +
                            '" alt="kanban-image">';
                        ("</div>");
                    }
                    // check if Badge is defined or not
                    if (typeof $(board_item_el).attr("data-badgeContent") !== "undefined") {
                        board_item_badge =
                            '<div class="kanban-badge">' +
                            '<div class="avatar bg-' +
                            kanban_board_data[kanban_data].item[kanban_item].badgeColor +
                            ' font-size-small font-weight-bold">' +
                            kanban_board_data[kanban_data].item[kanban_item].badgeContent +
                            "</div>";
                        ("</div>");
                    }
                    // add custom 'kanban-footer'
                    if (
                        typeof(
                            $(board_item_el).attr("data-dueDate") ||
                            $(board_item_el).attr("data-comment") ||
                            $(board_item_el).attr("data-users") ||
                            $(board_item_el).attr("data-attachment")
                        ) !== "undefined"
                    ) {
                        $(board_item_el).append(
                            '<hr/>' + '<div class="kanban-footer-left d-flex">' + board_item_degres + board_item_reclamation + board_item_dueDate + board_item_duree +  "</div>" + '<hr/>' + board_item_comment +'<hr><div class="kanban-footer d-flex justify-content-between mt-1">' +
                            '<div class="kanban-footer-left d-flex">' +
                            
                            board_item_attachment + board_item_client + 
                            "</div>" +
                            '<div class="kanban-footer-right">' +
                            '<div class="kanban-users">' +
                            board_item_badge +
                            '<ul class="list-unstyled users-list cursor-pointer m-0 d-flex align-items-center">' +
                            board_item_users +
                            "</ul>" +
                            "</div>" +
                            "</div>" +
                            "</div>"
                        );
                    }
                    // add Image prepend to 'kanban-Item'
                    if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                        $(board_item_el).prepend(board_item_image);
                    }
                }
            }
            kanban_board_data = [];
        });



    // Add new kanban board
    //---------------------
   if(ajaxResultCommencer.length>0){
$("[data-id~='kanban-board-1'] .kanban-board-header").css("background-color", "orange");

}

    // Delete kanban board
    //---------------------


    // Kanban board dropdown
    // ---------------------


    // Kanban-overlay and sidebar hide
    // --------------------------------------------
    $(
        ".kanban-sidebar .delete-kanban-item, .kanban-sidebar .close-icon, .kanban-sidebar .update-kanban-item, .kanban-overlay"
    ).on("click", function() {
        $(".kanban-overlay").removeClass("show");
        $(".kanban-sidebar").removeClass("show");
    });

    // Updating Data Values to Fields
    // -------------------------------
    $(".update-kanban-item").on("click", function(e) {
        e.preventDefault();
    });

    // Delete Kanban Item
    // -------------------
    $(".delete-kanban-item").on("click", function() {
        $delete_item = kanban_curr_item_id;
        addEventListener("click", function() {
            KanbanExample.removeElement($delete_item);
        });
    });



    // Making Title of Board editable
    // ------------------------------


    // kanban Item - Pick-a-Date
    $(".edit-kanban-item-date").pickadate();

    // Perfect Scrollbar - card-content on kanban-sidebar
    if ($(".kanban-sidebar .edit-kanban-item .card-content").length > 0) {
        var kanbanSidebar = new PerfectScrollbar(".kanban-sidebar .edit-kanban-item .card-content", {
            wheelPropagation: false
        });
    }

    // select default bg color as selected option
    $("select").addClass($(":selected", this).attr("class"));

    // change bg color of select form-control
    $("select").change(function() {
        $(this)
            .removeClass($(this).attr("class"))
            .addClass($(":selected", this).attr("class") + " form-control text-white");
    });

}


    $(".kanban-item").each(function () {
        var $element = $(this);
        var numReclamation = $element.data("numreclamation");
        var statusReclamation = $element.data("numreclamationstatus");
        var degres = $element.data("degres");
        if (numReclamation != "" && statusReclamation == "<span class='badge badge-warning'>Nouveau</span>") {
            // If data-numreclamation has a value, add a CSS class
            $element.addClass("bg-danger");
        } else if (numReclamation != "" && statusReclamation == "<span class='badge badge-success'>En Cours</span>") {
            // If data-numreclamation is empty or not present, add a different CSS class
            $element.addClass("bg-success");
        } else if (numReclamation != "" && statusReclamation == "<span class='badge badge-striped'>Cloturée</span>") {
            // If data-numreclamation is empty or not present, add a different CSS class
            $element.addClass("bg-light");
        }
        if (degres == "1") {
            // If data-numreclamation has a value, add a CSS class
            $element.addClass("bg-warning");
        } else if (degres == "2") {
            // If data-numreclamation is empty or not present, add a different CSS class
            $element.addClass("bg-default");
        } else if (degres == "3") {
            // If data-numreclamation is empty or not present, add a different CSS class
            $element.addClass("bg-light");
        }
    });

});