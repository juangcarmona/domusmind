namespace DomusMind.Domain.Calendar;

public sealed class CalendarEventAlreadyCancelledException(string message)
    : InvalidOperationException(message);
