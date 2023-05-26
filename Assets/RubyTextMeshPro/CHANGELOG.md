# Changelogs

## Version 1.1.1

- Removed `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText`.
- Add getter to `RubyTextMeshPro.uneditedText` / `RubyTextMeshProUGUI.uneditedText`.
- Add support to install via UPM Git URL.

## Version 1.1.0

- Removed:  
  `allVCompensationRuby` / `allVCompensationRubyLineHeight` : If `rubyLineHeight` is an empty string, it will be the `allVCompensationRuby:false` value up to now, and if `rubyLineHeight` is a value, it will be the `allVCompensationRubyLineHeight` value.
- Obsolete:  
  `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` will be removed in the next version. Please use `uneditedText`.
