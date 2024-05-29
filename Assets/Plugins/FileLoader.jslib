var FileLoader = {
    loadFile: function(callback) {
        var input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json'; // Specify the file types you want to allow
        input.onchange = function(event) {
            var file = event.target.files[0];
            if (file) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    var contents = e.target.result;
                    // Ensure that the callback is a function before calling it
                    if (typeof callback === 'function') {
                        callback(contents);
                    } else {
                        console.error('Callback is not a function');
                    }
                };
                reader.readAsText(file);
            }
        };
        input.click();
    }
};
mergeInto(LibraryManager.library, FileLoader);
