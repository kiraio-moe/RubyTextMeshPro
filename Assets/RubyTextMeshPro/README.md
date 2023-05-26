# Ruby (Furigana) TextMeshPro

![RubyTextMeshPro Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/ruby.png)

RubyTextMeshPro is a plugin to add ruby tags to Unity TextMeshPro to support writing Kanji.

## Features

- Realtime Ruby text preview.
- Support both TextMeshPro (3D) and TextMeshProUGUI (UI).

## How To Use

- Install this plugin via Package Manager Git URL:

  ```txt
  https://github.com/kiraio-moe/RubyTextMeshPro.git?path=Assets/RubyTextMeshPro
  ```

  or the classic way with `.unitypackage` from [Releases](https://github.com/kiraio-moe/RubyTextMeshPro/releases "Releases").  
  If you install using `.unitypackage`, don't forget to install TextMeshPro via Package Manager.

- Create new RubyTextMeshPro GameObject of your choice.
- Start writing the Ruby tag.  
  You can use `<ruby>` or `<r>` tag, both tags are valid.

  ```txt
  <ruby=ice>fire</ruby>
  <r=ice>fire</r>
  ```

  It also work with double quotes:

  ```txt
  <ruby="ice">fire</ruby>
  <r="ice">fire</r>
  ```

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/add_ruby.gif)

## Usage Description

`<ruby=かんじ>漢字</ruby>`

### RubyShowType

- RUBY_ALIGNMENT: Display text according to the ruby.
- BASE_ALIGNMENT: Display characters according to the original characters.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/align_width.gif)

### rubyLineHeight

This function allows you to have the same gap even if you don't use ruby.
Empty this string to skip this feature.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/vcompensation.gif)

## Disruptive Changes

### Version 1.1.0

- Removed:  
  `allVCompensationRuby` / `allVCompensationRubyLineHeight` : If `rubyLineHeight` is an empty string, it will be the `allVCompensationRuby:false` value up to now, and if `rubyLineHeight` is a value, it will be the `allVCompensationRubyLineHeight` value.

Read more on [Changelogs](./CHANGELOG.md).

## Known Issues

1. TextMeshPro source has not changed. So text alignment is problematic.
2. Do not make the text box smaller than the maximum number of characters in ruby. Display collapse will occur.

3. `BASE_ALIGN` setting is left align and the ruby is at the beginning of the line but more than the original character, it will be displayed outside the frame.

  ![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_lefttop.png)

4. `BASE_ALIGN` setting is center align and the ruby is at more than the original character, can't displayed center. 'RUBY_ALIGN' used, it may be solved.

  ![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_center.png)

5. `BASE_ALIGN` setting is left align and the ruby is at the beginning of the line but more than the original character, Different from number 3 it will be displayed in the frame.

  ![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_bottomright.png)

## Resources

**Used font**  
[Rounded M+](http://jikasei.me/font/rounded-mplus "Rounded M+")

**References**  
- <https://forum.unity.com/threads/how-to-display-extra-little-characters-above-characters-in-a-text.387772/>
- <http://baba-s.hatenablog.com/entry/2019/01/10/122500>

Thank You!

## Notes

- TextMeshPro is a very nice  plugin. If in the future they add ruby tag very well, we will delete this project.
- Not checked anything other than Japanese.

## Contributing

All contributions are welcomed. Just make sure you follow the project's code style.  

Contact: jenomoto@netsis.jp

## License

This project is licensed under [MIT License](/LICENSE "Read LICENSE file")
