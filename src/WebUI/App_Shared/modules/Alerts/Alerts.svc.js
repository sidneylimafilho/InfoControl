(function($) {
    window.InitAlertMonitor = function(Alerts) {
        if (!top.document.originalTitle)
            top.document.originalTitle = top.document.title;

        Alerts = Alerts || {};

        if (!Alerts.Count)
            Alerts.Count = 0;
        if (!Alerts.LastAlertId)
            Alerts.LastAlertId = 0;

        Alerts.SetTitle = function(count) {
            top.document.title = "Você tem " + count + " mensagens!";
            top.focus();
            if (count == 0)
                Alerts.RestoreTitle();
        }

        Alerts.RestoreTitle = function() {
            top.document.title = top.document.originalTitle;
        }

        Alerts.timerId = setTimeout(function() {
            Alerts.GetAlerts(function(result, response, context) {
                result = eval(result);

                for (var i = 0; i < result.length; i++) {
                    if (Alerts.LastAlertId < result[i].AlertId) {
                        top.$.jGrowl(result[i].Description, {
                            id: result[i].AlertId,
                            beforeClose: function(e, m, o) {
                                Alerts.Delete(o.id);
                                Alerts.SetTitle(--Alerts.Count);
                            }
                        });
                        Alerts.LastAlertId = result[i].AlertId;
                        Alerts.SetTitle(++Alerts.Count);
                    }
                }

                InitAlertMonitor();
            }
        )
        }, 3000);
    }
})(jQuery);
