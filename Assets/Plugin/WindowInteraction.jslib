var WindowInteraction = {
    openUrl: function(link)
    {
        var url =  Pointer_stringify(link);
        document.onmouseup = function()
        {
            window.open(url);
            document.onmouseup = null;
        }
    }
};

mergeInto(LibraryManager.library, WindowInteraction);