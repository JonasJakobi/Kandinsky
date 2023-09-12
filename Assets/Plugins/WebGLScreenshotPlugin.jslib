mergeInto(LibraryManager.library, {
  WebGLTakeScreenshot: function() {
    var canvas = document.querySelector('canvas');
    var dataUrl = canvas.toDataURL('image/png');
    var link = document.createElement('a');
    link.href = dataUrl;
    link.download = 'screenshot.png';
    link.click();
  }
});