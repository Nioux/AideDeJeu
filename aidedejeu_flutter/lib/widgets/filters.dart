import 'package:aidedejeu_flutter/theme.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class RangeFilter extends StatefulWidget {
  final List<String> values;
  final RangeValues rangeValues;
  final ValueChanged<RangeValues> updateRangeValues;

  RangeFilter(
      {Key key, @required this.values, this.rangeValues, this.updateRangeValues}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _RangeFilterState();
  }
}

class _RangeFilterState extends State<RangeFilter> {
  @override
  Widget build(BuildContext context) {
    return RangeSlider(
      activeColor: colorHDRed,
        inactiveColor: colorHDLightGrey,
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

  ChipListFilter({Key key, this.choices, this.selectedChoices, this.updateSelectedChoices}) : super(key: key);

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
      spacing: 0.0,
        crossAxisAlignment: WrapCrossAlignment.start,
        alignment: WrapAlignment.spaceEvenly,
        direction: Axis.horizontal,
        runAlignment: WrapAlignment.start,
        verticalDirection: VerticalDirection.down,

        children: widget.choices
            .map((choice) => Padding(
                padding: const EdgeInsets.fromLTRB(2,0,2,0),
                child: FilterChip(
                  label: Text(choice, style: TextStyle(fontSize: 10.0),),
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

  ChipFilter({Key key, this.label}) : super(key: key);

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
