# MCDA.NET

.NET Port of [Python 3 library](https://gitlab.com/shekhand/mcda/) for solving multi-criteria decision-making (MCDM) problems.

## Implemented methods
The library contains:

|  Acronym            	|  Method Name                                                                   	|                Reference               	|
| :-------------------- | -------------------------------------------------------------------------------   | :--------------------------------------:	|
|  TOPSIS             	|  Technique for the Order of Prioritisation by Similarity to Ideal Solution     	|               [[1]](#c1)               	|
|  VIKOR              	|  VIseKriterijumska Optimizacija I Kompromisno Resenje                          	|               [[2]](#c2)               	|
|  SPOTIS             	|  Stable Preference Ordering Towards Ideal Solution                             	|               [[3]](#c3)               	|
|  MABAC              	|  Multi-Attributive Border Approximation area Comparison                        	|               [[4]](#c4)              	|

## Usage example
```
var matrix = new NDArray(new double[,]
{
    { 78, 56, 34, 6 },
    { 4, 45, 3, 97 },
    { 18, 2, 50, 63 },
    { 9, 14, 11, 92 },
    { 85, 9, 100, 29 }
});

var weights = new NDArray(new double[] { 0.25, 0.25, 0.25, 0.25 });
var types = new NDArray(new double[] { 1, 1, 1, 1 });

var vikor = new VikorMethod(matrix, weights, types, (m, cost) => m);

var values = vikor.Resolve();
// [0.5679, 0.7667, 1, 0.7493, 0]
```

## Screenshots of sample program

![TOPSIS and VIKOR results](https://i.imgur.com/UMArTqr.png)
![MABAC and SPOTIS results](https://i.imgur.com/qVkyYNM.png)

# References

<a name="c1">**[1]**</a> Hwang, C. L., & Yoon, K. (1981). Methods for multiple attribute decision making. In Multiple attribute decision making (pp. 58-191). Springer, Berlin, Heidelberg.

<a name="c2">**[2]**</a> Duckstein, L., & Opricovic, S. (1980). Multiobjective optimization in river basin development. Water resources research, 16(1), 14-20.

<a name="c3">**[3]**</a> Dezert, J., Tchamova, A., Han, D., & Tacnet, J. M. (2020, July). The spotis rank reversal free method for multi-criteria decision-making support. In 2020 IEEE 23rd International Conference on Information Fusion (FUSION) (pp. 1-8). IEEE.

<a name="c4">**[4]**</a> Pamučar, D., & Ćirović, G. (2015). The selection of transport and handling resources in logistics centers using Multi-Attributive Border Approximation area Comparison (MABAC). Expert systems with applications, 42(6), 3016-3028.