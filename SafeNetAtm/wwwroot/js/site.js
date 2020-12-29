function ErrorAlert(alertID, errMsg) {
    $("#" + alertID).alert("close");
    var html = "";
    html = "<div id='" + alertID + "' class='alert alert-danger alert-dismissible' style='display:none;margin-bottom:0px;'>";
    html += "<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>";
    html += "<kbd>Alert:</kbd>  <b>" + msg + "</b></div>";
    $("#divGeneralAffixMSG").append(html);
    $("#" + alertID).fadeIn();
}