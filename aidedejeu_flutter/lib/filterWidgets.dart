import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class RangeFilter extends StatefulWidget {
  Filter filter;

  RangeFilter({@required this.filter});

  @override
  State<StatefulWidget> createState() {
    // TODO: implement createState
    return _RangeFilter(filter: filter);
  }
}

class _RangeFilter extends State<RangeFilter> {
  Filter filter;
  RangeValues rangeValues;
  _RangeFilter({@required this.filter}) {
    rangeValues = RangeValues(0, filter.values.length.toDouble() - 1);
  }

  @override
  Widget build(BuildContext context) {

    return RangeSlider(
        min: 0,
        max: filter.values.length.toDouble() - 1,
        divisions: filter.values.length,
        labels: RangeLabels(
            '${filter.values[rangeValues.start.round()]}', '${filter.values[rangeValues.end.round()]}'),
        values: rangeValues ?? RangeValues(0, filter.values.length.toDouble() - 1),
        onChanged: (RangeValues values) {
          setState(() {
            rangeValues = values;
          });
        });
  }
}