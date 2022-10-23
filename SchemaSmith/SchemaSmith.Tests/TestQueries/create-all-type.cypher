CREATE (n:ValueTypesNode)
SET n = {
  nullValue: null,
  intList: [ 1, 2, 3],
  boolean: true,
  int: 1,
  float: 1.001,
  string: "foo",
  point: point({ latitude: 12, longitude: 56 }),
  timestamp: timestamp(),
  dateForLocalDateTime: date(),
  dateForDateTime: date(),
  dateForLocalDate: date(),
  time: time(),
  localTime: localtime(),
  localTimeForTimeSpan: localtime(),
  localTimeForDateTime: localtime(),
  dateTimeForZonedDateTime: datetime(),
  dateTimeForDateTimeOffset: datetime(),
  localDateTime: localdatetime(),
  localDateTimeForDateTime: localdatetime(),
  duration: duration('P14DT16H12M'),
  floatForString: 1.002
}