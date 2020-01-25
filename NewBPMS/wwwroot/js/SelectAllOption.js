$(document).ready(function () {
    $("#selectAllOption").click(function () {
        if (this.checked) {
            $(":checkbox[name^=ItemChosen]").prop("checked", true);
        }
        else {
            $(":checkbox[name^=ItemChosen]").prop("checked", false);
        }
    });
});