
import 'package:flutter/material.dart';

enum FilterType {
  Choices, Range
}
class Filter {
  String name;
  String displayName;
  FilterType type;
  List<String> values;
  Set<String> selectedValues = Set<String>();
  RangeValues rangeValues;

  Filter({this.name, this.type, this.values, this.displayName}) {
    rangeValues = RangeValues(0, values.length.toDouble() - 1);
  }
}

