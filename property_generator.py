import logging
import sys

type_dict = {
    "string": "\"string\"",
    "integer": "0",
    "float": "1.0",
    "boolean": "false",
    "point": "point({ latitude: 0, longitude: 0 })",
    "date": "date()",
    "time": "time()",
    "localTime": "localtime()",
    "dateTime": "datetime()",
    "localDateTime": "localdatetime()",
    "duration": "duration('P14DT16H12M')",
    "list(string)": "[ \"string\" ]",
    "list(integer)": "[ 0 ]",
    "list(float)": "[ 1.0 ]",
    "list(boolean)": "[ false ]",
    "list(point)": "[ point({ latitude: 0, longitude: 0 }) ]",
    "list(date)": "[ date() ]",
    "list(time)": "[ time() ]",
    "list(localTime)": "[ localtime() ]",
    "list(dateTime)": "[ datetime() ]",
    "list(localDateTime)": "[ localdatetime() ]",
    "list(duration)": "[ duration('P14DT16H12M') ]"
}


def generate_property(property_type):
    try:
        return type_dict[property_type]
    except Exception as err:
        logging.exception(err)
        sys.exit("SchemaSmith failed. Because you did.")
