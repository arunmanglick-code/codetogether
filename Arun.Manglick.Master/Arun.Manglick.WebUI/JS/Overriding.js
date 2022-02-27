
// Base Class

function Base()
{
    this.OverrideMe = function()
    {
        alert("Base::Override()");
    }

    this.BaseFunction = function()
    {
        alert("Base::BaseFunction()");
    }
}

// Derive Class

function Derive()
{
    this.OverrideMe = function()
    {
        alert("Derive::Override()");
    }
}

// Client Class

function CallOveride()
{
    Derive.prototype = new Base(); // Required only when u want to call some base class fucntions as well. This must be the first statement.
    
    d = new Derive();
    d.OverrideMe();  // Derive Class
    d.BaseFunction(); // Base Class
    
    d = new Base();
    d.OverrideMe();  // Base Class

}

