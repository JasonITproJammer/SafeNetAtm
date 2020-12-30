function Alert(alertID, msg, type) {
    $("#" + alertID).alert("close");
    var html = "";
    html = "<div id='" + alertID + "' class='alert alert-" + type
        + " alert-dismissible w-100' style='display:none;margin-bottom:0px;'>";
    html += "<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>";
    html += "<kbd>Alert:</kbd>  <b>" + msg + "</b></div>";
    $("#divGeneralAffixMSG").append(html);
    $("#" + alertID).fadeIn();
}

function AtmActionChange() {
    var errMsg = "none";
    var selOption = $('#ddlAtmAction').find(":selected").val();
    switch (selOption) {
        case "I":
            var test = $('#ddlDenoms').val();
            if (test.length < 1) {
                errMsg = "You must select at least one denomination.";
            }
            break;
        default:
            errMsg = "Invalid drop down item chosen.";
    }

    if (errMsg == "none") {
        var antiForgeryToken = $('#divHomeIndex input[name="__RequestVerificationToken"]').val();
        $("#colResults").empty();
        switch (selOption) {
            case "B":
                //Home - CurrentBalance
                var model = {};
                model["__RequestVerificationToken"] = antiForgeryToken;
                $.ajax({
                    type: "POST",
                    url: $("#urlCurrentBalance").val(),
                    data: model,
                    success: function (response) {
                        $('#colResults').html(response);
                    },
                    error: function (response) {
                        $('#colResults').html(response);
                    }
                });
                break;
            case "I":
                //Home - DenominationBalance
                var model = {};
                model["__RequestVerificationToken"] = antiForgeryToken;
                model["Denomination"] = $('#ddlDenoms').val();
                $.ajax({
                    type: "POST",
                    url: $("#urlDenominationBalance").val(),
                    data: model,
                    success: function (response) {
                        $('#colResults').html(response);
                    },
                    error: function (response) {
                        $('#colResults').html(response);
                    }
                });
                break;
            case "W":
                //Home - Withdraw
                var amountOrig = $('#numWithdraw').val();
                var amountNumsOnly = amountOrig.replace(/\D/g, ""); //remove all non numeric values
                var model = {};
                model["__RequestVerificationToken"] = antiForgeryToken;
                model["WithdrawalAmount"] = amountNumsOnly;
                $.ajax({
                    type: "POST",
                    url: $("#urlWithdraw").val(),
                    data: model,
                    success: function (response) {
                        if ($(response).find("#errorFlag").length < 1) {
                            Alert("SuccessWithdraw", "Success: Dispensed " + amountOrig, "success");
                        }
                        $('#colResults').html(response);
                    },
                    error: function (response) {
                        $('#colResults').html(response);
                    }
                });
                break;
            case "R":
                //Home - Restock
                var model = {};
                model["__RequestVerificationToken"] = antiForgeryToken;
                $.ajax({
                    type: "POST",
                    url: $("#urlRestock").val(),
                    data: model,
                    success: function (response) {
                        $('#colResults').html(response);
                    },
                    error: function (response) {
                        $('#colResults').html(response);
                    }
                });
                break;
            default:
                Alert("validationAtmActionChange", "Invalid drop down item chosen.", "danger");
        }
    }
    else {
        Alert("validationAtmActionChange", errMsg, "danger");
    }
}