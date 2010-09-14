
$(function () {
    $("#aspnetForm").validate({
        onsubmit: false,
        rules: {
            // ctl00$ContentPlaceHolder$selContact$txtContact: "required",
            ctl00$ContentPlaceHolder$txtDescription: "required"
        },
        messages: {
            //  ctl00$ContentPlaceHolder$selContact$txtContact: "&nbsp&nbsp <div class='cErr21' style='color:Red;'> &nbsp&nbsp&nbsp  </div>",
            ctl00$ContentPlaceHolder$txtDescription: "&nbsp&nbsp <div  class='cErr21' style='color:Red;'> &nbsp&nbsp&nbsp  </div>"
        }
    });

    $("#ctl00_ContentPlaceHolder_btnSave").click(function () {

        if ($("#aspnetForm").valid()) {

            var customerFollowUp = [];

            customerFollowUp[0] = $("#ctl00_ContentPlaceHolder_txtContactId").val();
            customerFollowUp[1] = $("#ctl00_ContentPlaceHolder_companyId").val();
            customerFollowUp[2] = $("#ctl00_ContentPlaceHolder_userId").val();

            customerFollowUp[3] = $("#ctl00_ContentPlaceHolder_txtDescription").val();
            customerFollowUp[4] = $("#ctl00_ContentPlaceHolder_cboCustomerFollowupAction").val();

            var date = $("#ctl00_ContentPlaceHolder_ucNextMeetingDate_txtDate").val();
            var time = $("#ctl00_ContentPlaceHolder_ucNextMeetingDate_cboTime option:selected").text() + ":00";

            if ($("#ctl00_ContentPlaceHolder_txtContactId").val() == "") {
                $("#ctl00_ContentPlaceHolder_reqSelContact").attr("style", "display:block");
                return false;
            }

            if (date != "" && $("#ctl00_ContentPlaceHolder_txtAppoitmentSubject").val() == "") {
                $("#reqTxtAppoitmentSubject").attr("style", "display:inline");
                return false;
            }

            if (date == "" && $("#ctl00_ContentPlaceHolder_txtAppoitmentSubject").val() != "") {
                $("#ctl00_ContentPlaceHolder_ucNextMeetingDate_reqtxtDate").attr("style", "display:inline");
                return false;
            }

            customerFollowUp[6] = date + " " + time;
            customerFollowUp[7] = $("#ctl00_ContentPlaceHolder_cboContact option:selected").text();

            customerFollowUp[8] = $("#ctl00_ContentPlaceHolder_customerFollowUpId").val();
            customerFollowUp[9] = $("#ctl00_ContentPlaceHolder_userId").val();
            customerFollowUp[10] = $("#ctl00_ContentPlaceHolder_txtAppoitmentSubject").val();

            PageMethods.SaveCustomerFollowUp(customerFollowUp,
            function (result) {
                if (result)
                    document.location = "CustomerFollowUps.aspx";
            });
        }
    });

});
