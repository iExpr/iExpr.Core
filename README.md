<div align="center">
    <img src="./resources/images/core.png" width = "8%"/>
    <h1>iExpr.Core</h1>
</div>

![](https://img.shields.io/badge/framework-.netstandard2.0-blue.svg)
![](https://img.shields.io/badge/build-passing-brightgreen.svg)
![](https://img.shields.io/badge/alpha-v0.5.2.7-blue.svg)
[![](https://img.shields.io/badge/nuget-v0.5.2.2-brightgreen.svg)](https://www.nuget.org/packages/iExpr.Core/0.5.2.2)
![](http://progressed.io/bar/80?title=done)

+ Author: Stardust D.L.
+ Version: 0.5.2

The core types and functions of iExpr. 
> iExpr is an extensible expression parser and evaluator.

> Current version is coding thesedays.

# Functions

+ You can use **iExpr.Core** to define your own expression types by creating different operations.  
+ Also you can use it to *"create"* a simple programming language. 

    > PS: Please see [ExprSharp](https://github.com/ExprSharp)

# Install

```
PM> Install-Package iExpr.Core -Version 0.5.2.2
```

# Brief Usage

1. Install the package from nuget
2. Define your operations and functions
3. implement your own **ParseEnvironment** based on `iExpr.Parsers.ParseEnvironment`
3. implement your own **EvalEnvironment** based on `iExpr.Evaluators.EvalEnvironment`
4. Use the code below to evaluate your expression:
    ```cs
    ParseEnvironment ep = new YourParseEnvironment();
    EvalEnvironment ep = new YourEvalEnvironment();
    ExprBuilder eb = new ExprBuilder(ep);
    var buildedExpr = eb.GetExpr(exprString);
    var context = ev.CreateContext().GetChild();
    var evaluatedExpr=context.Evaluate(buildedExpr);
    ```
+ You can go to `iExpr.Exprs` to see how to implement your own environment and operations. [Link](https://github.com/iExpr/iExpr.Exprs)
    + The logic project is a simple example
    + For a complex one, you can see [ExprSharp](https://github.com/ExprSharp)

# Note

The version 0.5 is developing. I have changed many things from v0.3 to improve its extensibility. 
Some APIs have been changed. Maybe you can find them in wiki some days later (but it's still **empty** now).

# Links

This is the app on Windows that I designed:
[iExpr](https://stardustdl.github.io/Blog/2017/12/22/%E8%A1%A8%E8%BE%BE%E5%BC%8F%E8%AE%A1%E7%AE%97%E5%99%A8-iExpr/)

# License

## LGPLv3