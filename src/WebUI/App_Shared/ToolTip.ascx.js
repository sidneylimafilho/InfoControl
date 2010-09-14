if(!window.InfoControl)window.InfoControl = {};
window.InfoControl.ToolTip = function(_outerDiv, _btnFechar, _btnPersist, _nextToolTip)
{
    this.outerDiv = _outerDiv;
    this.btnFechar = _btnFechar;
    this.btnPersist = _btnPersist;
    this.NextToolTip = _nextToolTip;
            
    _outerDiv.Show=function(){}
    
    $addHandler(_btnFechar, "click", function()
    {        
        if(_btnPersist.checked)
        {
            top.infocontrol.tooltipservice.SetToolTipClosed(_outerDiv.getAttribute('page'), _outerDiv.id);            
        }
            
        if(this.NextToolTip)
            this.NextToolTip.Show();
    });
}

