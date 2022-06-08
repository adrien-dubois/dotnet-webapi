
$(document).ready(function () {
    "console" == $(".choix_console_accessoire").val()
        ? ($(".choix_console").show(), $(".choix_accessoire").hide())
        : "accessoire" == $(".choix_console_accessoire").val()
        ? ($(".choix_accessoire").show(), $(".choix_console").hide())
        : ($(".choix_console").hide(), $(".choix_accessoire").hide()),
        $("#sav_garantie").is(":checked") && $("#sav_conditions_generales_ventes").is(":checked") && $("#sav_save").removeAttr("disabled"),


        $(".choix_console_accessoire").change(function (e) {
            "console" == $(this).val()
                ? ($(".choix_console").show(), $(".choix_accessoire").hide(), $(".legend_game_watch").hide())
                : "accessoire" == $(this).val()
                ? ($(".choix_accessoire").show(), $(".choix_console").hide())
                : ($(".choix_console").hide(), $(".choix_accessoire").hide());
        }),

        $("#sav_console").change(function (e) {
            "12" == $(this).val() || "13" == $(this).val()
                ? $(".legend_game_watch").show()
                : $(".legend_game_watch").hide();
        }),
        $("#sav_code_postal").change(function (e) {
            console.log("change cp"), $(this).val().startsWith("97") && (console.log("code postal"), $("#codePostalModal").modal(), $(this).val(""));
        }),
        $("#sav_conditions_generales_ventes").click(function (e) {
            1 == $("#sav_garantie").is(":checked") && $(this).is(":checked") ? $("#sav_save").removeAttr("disabled") : $("#sav_save").attr("disabled", "disabled");
        }),
        $("#sav_garantie").click(function (e) {
            1 == $("#sav_conditions_generales_ventes").is(":checked") && $(this).is(":checked") ? $("#sav_save").removeAttr("disabled") : $("#sav_save").attr("disabled", "disabled");
        });
});

