var needleTail = {
    url: '',
    initialize: function (twoWayControllerUrl) {
        this.url = twoWayControllerUrl;
        if (!!window.EventSource) {
            var source = new EventSource(this.url);
            source.addEventListener('message', function (e) { needleTail.executeServerCall(e); }, false);
            source.addEventListener('open', function (e) { needleTail.connectionOpen(e); }, false);
            source.addEventListener('error', function (e) { needleTail.connectionError(e); }, false);
        } else {
            alert('SSE not supported');
        }
    },
    executeServerCall: function (e) {
        //convert string to json
        var call = eval("(" + e.data + ")");
        //the call is on e.data
        var method = call.command;
        var pars = call.parameters;
        needleTail.executeFunction(method, window, pars);

    },
    connectionOpen: function (e) { /* run any code when the connection is opened */ },
    connectionError: function (e) { /* run code when a connection fails */ },
    executeFunction: function (functionName, context, args) {
        var namespaces = functionName.split(".");
        var func = namespaces.pop();
        for (var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }

        return context[func].apply(context, args);
    }
};




