// Save a reference to the original method.
var _windowClose = window.close;

// Re-implement window.open
window.close = function ()
{
    window.open("","_self");
    _windowClose();
}
