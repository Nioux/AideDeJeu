import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class RangeFilter extends StatefulWidget {
  final List<String> values;
  final RangeValues rangeValues;
  final ValueChanged<RangeValues> updateRangeValues;

  RangeFilter(
      {@required this.values, this.rangeValues, this.updateRangeValues});

  @override
  State<StatefulWidget> createState() {
    return _RangeFilterState();
  }
}

class _RangeFilterState extends State<RangeFilter> {
  @override
  Widget build(BuildContext context) {
    return RangeSlider(
        min: 0,
        max: widget.values.length.toDouble() - 1,
        divisions: widget.values.length,
        labels: RangeLabels(
            '${widget.values[widget.rangeValues.start.round()]}',
            '${widget.values[widget.rangeValues.end.round()]}'),
        values: widget.rangeValues,
        onChanged: (RangeValues values) {
          setState(() {
            widget.updateRangeValues(values);
          });
        });
  }
}

class ChipListFilter extends StatefulWidget {
  final List<String> choices;
  final Set<String> selectedChoices;
  final ValueChanged<Set<String>> updateSelectedChoices;

  ChipListFilter({this.choices, this.selectedChoices, this.updateSelectedChoices});

  @override
  State<StatefulWidget> createState() {
    return _ChipListFilterState();
  }
}

class _ChipListFilterState extends State<ChipListFilter> {
  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Wrap(
        children: widget.choices
            .map((choice) => Padding(
                padding: const EdgeInsets.all(4.0),
                child: FilterChip(
                  label: Text(choice),
                  backgroundColor: Colors.transparent,
                  shape: StadiumBorder(side: BorderSide()),
                  selected: widget.selectedChoices.contains(choice),
                  onSelected: (bool value) {
                    print("selected");
                    var selectedChoices = widget.selectedChoices.toSet();
                    setState(() {
                      if (value) {
                        selectedChoices.add(choice);
                      } else {
                        selectedChoices.remove(choice);
                      }
                      widget.updateSelectedChoices(selectedChoices);
                    });
                  },
                )))
            .toList());
  }
}

class ChipFilter extends StatefulWidget {
  final String label;

  ChipFilter({this.label});

  @override
  State<StatefulWidget> createState() {
    return _ChipFilterState();
  }
}

class _ChipFilterState extends State<ChipFilter> {
  bool selected = true;

  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    return FilterChip(
      label: Text(widget.label),
      backgroundColor: Colors.transparent,
      shape: StadiumBorder(side: BorderSide()),
      selected: selected,
      onSelected: (bool value) {
        print("selected");
        setState(() {
          this.selected = value;
        });
      },
    );
  }
}
